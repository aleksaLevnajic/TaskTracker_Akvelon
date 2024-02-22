function Get_Task_By_Id{
    param (
        [string]$LocalhostUrl,
        [string]$ContentType        
    )
    $result = Invoke-RestMethod -Method GET -ContentType $ContentType -Uri $LocalhostUrl

    return $result
}
$taskId = 1
$localhostUrl = "https://localhost:7054/api/Task/" + $taskId
$contentType = 'Application/Json'

$responseGetTasks = Get_Tasks -LocalhostUrl $localhostUrl -ContentType $contentType
$responseGetTasks