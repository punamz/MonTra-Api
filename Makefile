COMPOSE := docker-compose

build: 
	$(COMPOSE) build --no-cache

up: ## boot all containers
	$(COMPOSE) up -d


down: ## Kill all containers
	$(COMPOSE) kill