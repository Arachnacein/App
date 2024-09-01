# building part
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# installers part
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"
COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

# production part
FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app
COPY --from=build-env /app/out .
COPY --from=build-env /root/.dotnet/tools /root/.dotnet/tools
ENV PATH="$PATH:/root/.dotnet/tools"
EXPOSE 8080
ENTRYPOINT ["dotnet", "BudgetManager.dll"]



##building part
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
#WORKDIR /app
#
## installers part
#RUN dotnet tool install --global dotnet-ef
#ENV PATH="$PATH:/root/.dotnet/tools"
#COPY . ./
#RUN dotnet restore
#RUN dotnet publish -c Release -o out
#
##production part
#FROM mcr.microsoft.com/dotnet/sdk:8.0
#WORKDIR /app
#COPY --from=build-env /app/out .
#EXPOSE 8080
#ENTRYPOINT ["dotnet", "BudgetManager.dll"]