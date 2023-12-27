#create mig
dotnet tool install --global dotnet-ef
dotnet tool update --global dotnet-ef
dotnet tool list –g
dotnet --version
dotnet --info

dotnet ef migrations add Initial --project DAL --startup-project WebApp --context ApplicationDbContext
--project DAL --startup-project WebApp --context AppDbContext

# correct migration
dotnet ef migrations add Initial --project DAL.EF.App --startup-project WebApp --context ApplicationDbContext
dotnet ef migrations add Token --project DAL.EF.App --startup-project WebApp --context ApplicationDbContext 


#apply mig
dotnet ef database update --project DAL --startup-project WebApp --context ApplicationDbContext

#scaffolding

dotnet aspnet-codegenerator controller -m Notifications -name NotificationsController -outDir Controllers -dc ApplicationDbContext -udl --referenceScriptLibraries -f


docker build -t study-group-finder .
docker run --name study-group-finder --rm -it -p 8000:80 study-group-finder


docker build -t study-group-finder .

