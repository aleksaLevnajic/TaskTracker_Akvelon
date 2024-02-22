$taskId = 6
$localhostUrlProject = "https://localhost:7054/api/Project/" + $taskId
$headersPost = Get_Authentication_Headers -Username $username -Password $password -LoginUrl $loginUrl -ContentType $contentType -RequestMethod "POST"
Invoke-RestMethod -Method DELETE -ContentType $contentType -Uri $localhostUrlProject -Headers $headersPost
