$localhostUrlProjectGet = "https://localhost:7054/api/Project"
$responseHeaders = Get_Authentication_Headers -Username $username -Password $password -LoginUrl $loginUrl -ContentType $contentType -RequestMethod "POST"

# Output response
Invoke-RestMethod -Method GET -ContentType $contentType -Uri $localhostUrlProjectGet -Headers $responseHeaders
Invoke-RestMethod -Method GET -ContentType $contentType -Uri $localhostUrlProjectGet"/1" -Headers $responseHeaders
