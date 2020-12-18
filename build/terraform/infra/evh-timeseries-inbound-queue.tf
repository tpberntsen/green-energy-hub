module "evhnm_timeseries_inbound_queue" {
  source                    = "../modules/event-hub-namespace"
  name                      = "evhnm-timeseries-inbound-queue-${var.environment}"
  resource_group_name       = data.azurerm_resource_group.greenenergyhub.name
  location                  = data.azurerm_resource_group.greenenergyhub.location
  sku                       = "Standard"
  capacity                  = 1
  tags                      = data.azurerm_resource_group.greenenergyhub.tags
}

module "evh_inboundqueue" {
  source                    = "../modules/event-hub"
  name                      = "evh-inbound-queue-${var.environment}"
  namespace_name            = module.evhnm_timeseries_inbound_queue.name
  resource_group_name       = data.azurerm_resource_group.greenenergyhub.name
  partition_count           = 32
  message_retention         = 1
  dependencies              = [module.evhnm_timeseries_inbound_queue.dependent_on]
}

module "evhar_inboundqueue_sender" {
  source                    = "../modules/event-hub-auth-rule"
  name                      = "evhar-inboundqueue-sender"
  namespace_name            = module.evhnm_timeseries_inbound_queue.name
  eventhub_name             = module.evh_inboundqueue.name
  resource_group_name       = data.azurerm_resource_group.greenenergyhub.name
  send                      = true
  dependencies              = [module.evh_inboundqueue.dependent_on]
}

module "evhar_inboundqueue_sender_connection_string" {
  source       = "../modules/key-vault-secret"
  name         = "evhar-inboundqueue-sender-connection-string"
  value        = module.evhar_inboundqueue_sender.primary_connection_string
  key_vault_id = module.kv_shared.id
  dependencies = [
      module.kv_shared.dependent_on, 
      module.evhar_inboundqueue_sender.dependent_on
  ]
}

module "evhar_inboundqueue_receiver" {
  source                    = "../modules/event-hub-auth-rule"
  name                      = "evhar-inboundqueue-receiver"
  namespace_name            = module.evhnm_timeseries_inbound_queue.name
  eventhub_name             = module.evh_inboundqueue.name
  resource_group_name       = data.azurerm_resource_group.greenenergyhub.name
  listen                    = true
  dependencies              = [module.evh_inboundqueue.dependent_on]
}

module "evhar_inboundqueue_receiver_connection_string" {
  source       = "../modules/key-vault-secret"
  name         = "evhar-inboundqueue-receiver-connection-string"
  value        = module.evhar_inboundqueue_receiver.primary_connection_string
  key_vault_id = module.kv_shared.id
  dependencies = [
      module.kv_shared.dependent_on, 
      module.evhar_inboundqueue_receiver.dependent_on
  ]
}

module "temp_valid_outbound" {
  source                    = "../modules/event-hub"
  name                      = "evh-valid-outbound-${var.environment}"
  namespace_name            = module.evhnm_timeseries_inbound_queue.name
  resource_group_name       = data.azurerm_resource_group.greenenergyhub.name
  partition_count           = 32
  message_retention         = 1
  dependencies              = [module.evhnm_timeseries_inbound_queue.dependent_on]
}

module "temp_valid_sender" {
  source                    = "../modules/event-hub-auth-rule"
  name                      = "evh-valid-outbound-sender"
  namespace_name            = module.evhnm_timeseries_inbound_queue.name
  eventhub_name             = module.temp_valid_outbound.name
  resource_group_name       = data.azurerm_resource_group.greenenergyhub.name
  send                      = true
  dependencies              = [module.temp_valid_outbound.dependent_on]
}

module "temp_valid_sender_connection_string" {
  source       = "../modules/key-vault-secret"
  name         = "temp-valid-sender-connection-string"
  value        = module.temp_valid_sender.primary_connection_string
  key_vault_id = module.kv_shared.id
  dependencies = [
      module.kv_shared.dependent_on, 
      module.temp_valid_sender.dependent_on
  ]
}

module "temp_invalid_outbound" {
  source                    = "../modules/event-hub"
  name                      = "evh-invalid-outbound-${var.environment}"
  namespace_name            = module.evhnm_timeseries_inbound_queue.name
  resource_group_name       = data.azurerm_resource_group.greenenergyhub.name
  partition_count           = 32
  message_retention         = 1
  dependencies              = [module.evhnm_timeseries_inbound_queue.dependent_on]
}

module "temp_invalid_sender" {
  source                    = "../modules/event-hub-auth-rule"
  name                      = "evh-invalid-outbound-sender"
  namespace_name            = module.evhnm_timeseries_inbound_queue.name
  eventhub_name             = module.temp_invalid_outbound.name
  resource_group_name       = data.azurerm_resource_group.greenenergyhub.name
  send                      = true
  dependencies              = [module.temp_invalid_outbound.dependent_on]
}

module "temp_invalid_sender_connection_string" {
  source       = "../modules/key-vault-secret"
  name         = "temp-invalid-sender-connection-string"
  value        = module.temp_invalid_sender.primary_connection_string
  key_vault_id = module.kv_shared.id
  dependencies = [
      module.kv_shared.dependent_on, 
      module.temp_invalid_sender.dependent_on
  ]
}