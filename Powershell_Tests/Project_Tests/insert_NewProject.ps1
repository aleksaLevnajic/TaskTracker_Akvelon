$localhostUrlProject = "https://localhost:7054/api/Project"
$requestBodyProject = @{
    name = "Task projects pws"
    startDate = "2025-02-24T11:47:55.815Z"
    endDate = "2025-02-26T11:47:55.815Z"
    priority = 1
}
$headersPost = Get_Authentication_Headers -Username $username -Password $password -LoginUrl $loginUrl -ContentType $contentType -RequestMethod "POST"
Invoke-RestMethod -Method POST -Body ($requestBodyProject | ConvertTo-Json) -ContentType $contentType -Uri $localhostUrlProject -Headers $headersPost
