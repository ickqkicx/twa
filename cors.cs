
Console.WriteLine("Hello asp.net");

var allow = "Allow";
var specific = "Specific";

builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => policy
    .Build());

    options.AddPolicy(allow, policy => policy
   .AllowAnyOrigin()
   .AllowAnyHeader()
   .AllowAnyMethod());

    options.AddPolicy(specific, policy => policy
    .WithOrigins("localhost")
    .AllowAnyHeader()
    .WithExposedHeaders("CustomHeader")
    .WithMethods(["GET", "POST", "PATCH", "DELETE"])
    .AllowCredentials());
});

//app.UseCors();
//app.UseCors(allow);
//app.UseCors(specific);

//[EnableCors(specific)]
//[DisableCors]
//.RequireCors(specific)