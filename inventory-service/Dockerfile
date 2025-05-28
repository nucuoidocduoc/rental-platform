FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
# Copy csproj và restore các package
COPY *.csproj ./
RUN dotnet restore

# Copy toàn bộ source code và build
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# ===== STAGE 2: RUNTIME =====
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy kết quả publish từ stage build sang
COPY --from=build /app/publish .
# Khởi động ứng dụng
ENTRYPOINT ["dotnet", "inventory-service.dll"]