
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "server",

            ValidateAudience = true,
            ValidAudience = "client",

            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("qwertyuiopasdfghjklzxcvbnm123456")),
        };
    });
builder.Services.AddAuthorization();


app.UseAuthentication();
app.UseAuthorization();