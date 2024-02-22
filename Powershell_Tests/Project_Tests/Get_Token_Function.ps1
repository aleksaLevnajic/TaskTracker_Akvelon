function Get_Authentication_Headers {
    param (
        [string]$Username,
        [string]$Password,
        [string]$LoginUrl,
        [string]$ContentType,
        [string]$RequestMethod
    )

    $loginBody = @{
        username = $Username 
        password = $Password
    }

    $token = Invoke-RestMethod -Method POST -Body ( $loginBody | ConvertTo-Json ) -ContentType $ContentType -Uri $LoginUrl

    $headers = @{
        "Authorization" = "Bearer " + $token
        "Content-Type" = $ContentType
    }

    return $headers
}

# Call the function
$username = "zikazikic"
$password = "zika123"
$loginUrl = "https://localhost:7054/api/Auth/login"
$contentType = "application/json"


$response = Get_Authentication_Headers -Username $username -Password $password -LoginUrl $loginUrl -ContentType $contentType -RequestMethod "POST"

# Output response
$response