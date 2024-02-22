function Update_Task {
    param(
        [System.Object]$ReqBody,
        [string]$LocalhostUrl,
        [string]$ContentType
    )
    $result = Invoke-RestMethod -Method PUT -Body ($ReqBody | ConvertTo-Json) -ContentType $ContentType -Uri $LocalhostUrl

    return $result
}

$taskForUpdateId = 13
$localhostUrl = "https://localhost:7054/api/Task/" + $taskForUpdateId
$requestBody = @{
    name = "Task powershell44"
    description = "Testing with powershell"
    priority = 1
    status = 1
    projectId = 1
}
$contentType = 'Application/Json'

$response = Update_Task -ReqBody $requestBody -LocalhostUrl $localhostUrl -ContentType $contentType
$response
