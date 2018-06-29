using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BlackMoonStudio.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlackMoonStudio
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            SearchServiceClient serviceClient = CreateSearchServiceClient(Configuration);

            Console.WriteLine("{0}", "Deleting index...\n");
            DeleteLessonsIndexIfExists(serviceClient);

            Console.WriteLine("{0}", "Creating index...\n");
            CreateLessonsIndex(serviceClient);

            ISearchIndexClient indexClient = serviceClient.Indexes.GetClient("lessons");

            Console.WriteLine("{0}", "Uploading documents...\n");
            UploadDocuments(indexClient);
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
        }

        private static SearchServiceClient CreateSearchServiceClient(IConfigurationRoot configuration)
        {
            string searchServiceName = configuration["SearchServiceName"];
            string adminApiKey = configuration["SearchServiceAdminApiKey"];
            SearchServiceClient serviceClient = new SearchServiceClient(searchServiceName, new SearchCredentials(adminApiKey));
            return serviceClient;
        }

        private static void DeleteLessonsIndexIfExists(SearchServiceClient serviceClient)
        {
            if (serviceClient.Indexes.Exists("lessons"))
            {
                serviceClient.Indexes.Delete("lessons");
            }
        }

        private static void CreateLessonsIndex(SearchServiceClient serviceClient)
        {
            var definition = new Index()
            {
                Name = "lessons",
                Fields = FieldBuilder.BuildForType<Lesson>()
            };
            serviceClient.Indexes.Create(definition);
        }

        private static void UploadDocuments(ISearchIndexClient indexClient)
        {
            var allLessons = new List<Lesson>();
            var beginnerLessons = Lesson.GetLessonsByCategory("beginner");
            var intermediateLessons = Lesson.GetLessonsByCategory("intermediate");
            var advancedLessons = Lesson.GetLessonsByCategory("advanced");

            allLessons.AddRange(beginnerLessons);
            allLessons.AddRange(intermediateLessons);
            allLessons.AddRange(advancedLessons);

            var indexActions = new List<IndexAction<Lesson>>();

            foreach (var lesson in allLessons)
            {
                var indexAction = IndexAction.Upload(lesson);
                indexActions.Add(indexAction);
            }

            var batch = IndexBatch.New(indexActions);

            try
            {
                indexClient.Documents.Index(batch);
            }
            catch (IndexBatchException e)
            {
                Console.WriteLine(
                    "Failed to index some of the documents: {0}",
                    String.Join(", ", e.IndexingResults.Where(r => !r.Succeeded).Select(r => r.Key)));
            }

            Console.WriteLine("Waiting for documents to be indexed...\n");
            Thread.Sleep(2000);
        }
    }
}
