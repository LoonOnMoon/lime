#!/bin/sh
migration=AutoMigration-$(date +'%Y-%m-%d--%H-%M-%S')

# Assembly definitions
web=src/Lime.Web
infrastructure=src/Lime.Infrastructure

# Pretty color coding
GREEN=$'\e[0;32m'
LIGHT_BLUE=$'\e[0;34m'
NC=$'\e[0m'

echo ${GREEN}Setting up ${migration}${NC}

### Identity Assembly
echo ${LIGHT_BLUE}Identity Assembly:${NC}

# Add a new migration for Identity Assembly
dotnet ef migrations add ${migration} --project ${infrastructure} --startup-project ${web} --context LimeIdentityDbContext

# Update database for Identity Assembly
dotnet ef database update --project ${infrastructure} --startup-project ${web} --context LimeIdentityDbContext

echo ${GREEN}Identity Assembly finish.${NC}

### Data Assembly
echo ${LIGHT_BLUE}Data Assembly:${NC}

# Add a new migration for Data Assembly
dotnet ef migrations add ${migration} --project ${infrastructure} --startup-project ${web} --context LimeDbContext

# Update database for Data Assembly
dotnet ef database update --project ${infrastructure} --startup-project ${web} --context LimeDbContext

echo ${GREEN}Data Assembly finish.${NC}
