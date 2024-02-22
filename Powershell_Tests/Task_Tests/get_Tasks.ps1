function Get_Tasks{
    param (
        [string]$LocalhostUrl,
        [string]$ContentType        
    )
    $result = Invoke-RestMethod -Method GET -ContentType $ContentType -Uri $LocalhostUrl

    return $result
}
$localhostUrl = "https://localhost:7054/api/Task"
$contentType = 'Application/Json'

$responseGetTasks = Get_Tasks -LocalhostUrl $localhostUrl -ContentType $contentType
$responseGetTasks




#Invoke-RestMethod -Method GET -ContentType $contentType -Uri $localhostUrl
#Invoke-RestMethod -Method GET -ContentType $contentType -Uri $localhostUrl"/1"

