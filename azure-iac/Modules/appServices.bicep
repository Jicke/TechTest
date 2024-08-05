param environment string = 'Dev'
param location string = resourceGroup().location

resource AppServicePlan 'Microsoft.Web/serverfarms@2020-06-01' = {
  name: '${environment}-tech-test-app-plan'
  location: location
  properties: {
    reserved: true
  }
  sku: {
    name: 'F1'
  }
  kind: 'windows'
}

  
resource TechTestApp 'Microsoft.Web/sites@2022-03-01' = {
  name: '${environment}-tech-test-app'
  kind: 'app'
  location: location
  properties: {
    serverFarmId: AppServicePlan.id
    siteConfig: {
      alwaysOn: true
    }
    httpsOnly: true
  }
}





