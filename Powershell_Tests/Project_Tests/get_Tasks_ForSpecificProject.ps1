$projectId = 1
$localhostUrlTasksForSpecProject = "https://localhost:7054/api/Project/tasks/" + $projectId
$responseHeaders = Get_Authentication_Headers -Username $username -Password $password -LoginUrl $loginUrl -ContentType $contentType -RequestMethod "POST"

# Output response
Invoke-RestMethod -Method GET -ContentType $contentType -Uri $localhostUrlTasksForSpecProject -Headers $responseHeaders
