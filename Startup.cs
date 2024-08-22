﻿// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.22.0

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using INFINIONGPT.Bots;
using Azure.AI.OpenAI;
using Azure;
using System;
using INFINIONGPT.Services.OpenAI;
using Microsoft.Bot.Builder.Azure.Blobs;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace INFINIONGPT
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        } 

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient().AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.MaxDepth = HttpHelper.BotMessageSerializerSettings.MaxDepth;
            });

            // Create the Bot Framework Authentication to be used with the Bot Adapter.
            services.AddSingleton<BotFrameworkAuthentication, ConfigurationBotFrameworkAuthentication>();

            //Create Transcript Logger
            var transcriptlogger = new BlobsTranscriptStore(
                dataConnectionString: Configuration.GetValue<string>("dataConnectionstring"),
                containerName: Configuration.GetValue<string>("ContainerName"));

            //services.AddSingleton<ITranscriptStore>(transcriptlogger);
            services.AddSingleton(new TranscriptLoggerMiddleware(transcriptlogger));

             
            //Create OpenAI Client 
            var client = new OpenAIClient(new Uri($"https://{Configuration.GetValue<string>("OpenAIName")}.openai.azure.com/"),
                new AzureKeyCredential(Configuration.GetValue<string>("OpenAIKey")));

            services.AddSingleton(client);

            services.AddSingleton<IOpenAIService, OpenAIService>();

            //Add Storage state
            services.AddSingleton<IStorage, MemoryStorage>();

            //Add State
            services.AddSingleton<UserState>();

            services.AddSingleton<ConversationState>();

            // Create the Bot Adapter with error handling enabled.
            services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();

            // Create the bot as a transient. In this case the ASP Controller is expecting an IBot.
            services.AddTransient<IBot, PaulBot>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles()
                .UseStaticFiles()
                .UseWebSockets()
                .UseRouting()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });

            // app.UseHttpsRedirection();
        }
    }
}
