$nodeVersion = node -v
$npmVersion = npm -v

Write-Host "Node.js version $nodeVersion is installed."
Write-Host "npm version $npmVersion is installed."

npm run build