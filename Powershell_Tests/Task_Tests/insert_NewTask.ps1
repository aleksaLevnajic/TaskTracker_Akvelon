function Insert_Task {
    param(
        [System.Object]$ReqBody,
        [string]$LocalhostUrl,
        [string]$ContentType
    )
    $result = Invoke-RestMethod -Method POST -Body ($ReqBody | ConvertTo-Json) -ContentType $ContentType -Uri $LocalhostUrl

    return $result
}

$localhostUrl = "https://localhost:7054/api/Task"
$requestBody = @{
    name = "Task powershell33"
    description = "Testing with powershell"
    priority = 1
    projectId = 1
    #status = 2
}
$contentType = 'Application/Json'

$response = Insert_Task -ReqBody $requestBody -LocalhostUrl $localhostUrl -ContentType $contentType
$response
