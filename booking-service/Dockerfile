FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
# Copy csproj và restore các package
ENV DOTNET_ENVIRONMENT=Development

COPY *.csproj ./
RUN dotnet restore

# Copy toàn bộ source code và build
COPY . ./
RUN dotnet publish -c Debug -o /app/publish

# ===== STAGE 2: RUNTIME =====
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
RUN curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l /vsdbg
WORKDIR /app

# Copy kết quả publish từ stage build sang
COPY --from=build /app/publish .
# Khởi động ứng dụng
ENTRYPOINT ["dotnet", "booking-service.dll"]