@description('Specify environment being deployed')
@allowed([
  'dev'
  'uat'
  'staging'
  'prod'
])
param environment string
param defaultLocation string = 'uksouth'

resource environmentResourceGroup 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  location: defaultLocation
  name: '${environment}-app-services'
}

module appServiceDeployment 'Modules/appServices.bicep' = {
  scope: environmentResourceGroup
  name: 'app-service-setup'
  params: {
    location: environmentResourceGroup.location
    environment: environment
  }
}

targetScope = 'subscription'
