#!/bin/sh
migration=AutoMigration-$(date +'%Y-%m-%d--%H-%M-%S')

# Assembly definitions
web=src/Lime.Web
identity=src/Lime.Infrastructure.Identity
data=src/Lime.Persistence

# Pretty color coding
GREEN=$'\e[0;32m'
LIGHT_BLUE=$'\e[0;34m'
NC=$'\e[0m'

echo ${GREEN}Setting up ${migration}${NC}

### Identity Assembly
echo ${LIGHT_BLUE}Identity Assembly:${NC}

# Add a new migration for Identity Assembly
dotnet ef migrations add ${migration} --project ${identity} --startup-project ${web} --context AuthDbContext

# Update database for Identity Assembly
dotnet ef database update --project ${identity} --startup-project ${web} --context AuthDbContext

echo ${GREEN}Identity Assembly finish.${NC}

### Data Assembly
echo ${LIGHT_BLUE}Data Assembly:${NC}

# Add a new migration for Data Assembly
dotnet ef migrations add ${migration} --project ${data} --startup-project ${web} --context LimeDbContext

# Update database for Data Assembly
dotnet ef database update --project ${data} --startup-project ${web} --context LimeDbContext

echo ${GREEN}Data Assembly finish.${NC}
