resource "azurerm_cosmosdb_account" "acc" {
  name                = "postoffice-${var.organisation}-${var.environment}"
  resource_group_name = data.azurerm_resource_group.greenenergyhub.name
  location            = data.azurerm_resource_group.greenenergyhub.location
  offer_type          = "Standard"
  kind                = "GlobalDocumentDB"
  # To enable global failover change to true and uncomment second geo_location
  enable_automatic_failover = false

  consistency_policy {
    consistency_level = "Session"
  }
  
  geo_location {
    location          = data.azurerm_resource_group.greenenergyhub.location
    failover_priority = 0
  }
  # geo_location {
  #   location          = "<secondary location>"
  #   failover_priority = 1
  # }
}

resource "azurerm_cosmosdb_sql_database" "db" {
  name                = "energinetDocsDB"
  resource_group_name = data.azurerm_resource_group.greenenergyhub.name
  account_name        = azurerm_cosmosdb_account.acc.name
}

resource "azurerm_cosmosdb_sql_container" "coll" {
  name                = "energinetDocsContainer"
  resource_group_name = var.resource_group_name
  account_name        = azurerm_cosmosdb_account.acc.name
  database_name       = azurerm_cosmosdb_sql_database.db.name
  partition_key_path  = "/RecipientMarketParticipant_mRID"
}

resource "azurerm_cosmosdb_sql_stored_procedure" "bulkPeek" {
  name                = "bulkPeek"
  resource_group_name = var.resource_group_name
  account_name        = azurerm_cosmosdb_account.acc.name
  database_name       = azurerm_cosmosdb_sql_database.db.name
  container_name      = azurerm_cosmosdb_sql_container.coll.name

  body = file("../../../spikes/postoffice/cosmos-db/bulkPeek.js")
}

resource "azurerm_cosmosdb_sql_stored_procedure" "bulkDelete" {
  name                = "bulkDelete"
  resource_group_name = var.resource_group_name
  account_name        = azurerm_cosmosdb_account.acc.name
  database_name       = azurerm_cosmosdb_sql_database.db.name
  container_name      = azurerm_cosmosdb_sql_container.coll.name

  body = file("../../../spikes/postoffice/cosmos-db/bulkDelete.js")
}

resource "azurerm_cosmosdb_sql_stored_procedure" "bulkPeekRejected" {
  name                = "bulkPeekRejected"
  resource_group_name = var.resource_group_name
  account_name        = azurerm_cosmosdb_account.acc.name
  database_name       = azurerm_cosmosdb_sql_database.db.name
  container_name      = azurerm_cosmosdb_sql_container.coll.name

  body = file("../../../spikes/postoffice/cosmos-db/bulkPeekRejected.js")
}

module "cosmosdb_account_primary_key" {
  source       = "../modules/key-vault-secret"
  name         = "cosmosdb-account-primary-key"
  value        = azurerm_cosmosdb_account.acc.primary_key
  key_vault_id = module.kv_shared.id
  dependencies = [
      module.kv_shared.dependent_on 
  ]
}