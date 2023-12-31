param($installPath, $toolsPath, $package, $project)

Set-StrictMode -version 2.0

##-------------------------------------------------
## Globals
##-------------------------------------------------
[string] $basePath = "Registry::HKEY_CURRENT_USER\Software\Xceed Software"
[string] $licensesPath = $basePath + '\' + 'Licenses'

[byte[]] $NbDaysBits = 2, 7, 12, 17, 22, 26, 31, 37, 42, 47, 51, 55, 59, 62, 0xFF
[byte[]] $ProductCodeBits = 3, 16, 29, 41, 53, 61, 0xFF
[byte[]] $ProductVersionBits = 4, 15, 25, 34, 43, 50, 58, 0xFF
[byte[]] $ChecksumBits = 0, 9, 18, 27, 36, 45, 54, 63, 0xFF
[string] $AlphaNumLookup = "ABJCKTDL4UEMW71FNX52YGP98Z63HRS0"
[int[]] $td1 = 0x7d0, 0xb, 0x11, 0x2d
[int[]] $td2 = 0x7d0, 0xc, 0x11, 0xf

        # (PackageID, PackageTrialKeysWebFormUrl, ..PackageProducts)
[string[][]] $PackagesMap = 
        ('Xceed.Products.Wpf.DataGrid.Base', '', 'DGP'),`
        ('Xceed.Products.Wpf.DataGrid.Full', '', 'DGP'),`
        ('Xceed.Products.Wpf.DataGrid.Themes', '', 'DGP'),`
        ('Xceed.Products.Wpf.Toolkit.Full', '', 'WTK'),`
        ('Xceed.Products.Wpf.Toolkit.AvalonDock', '', 'WTK'),`
        ('Xceed.Products.Wpf.Toolkit.AvalonDock.Themes', '', 'WTK'),`
        ('Xceed.Products.Wpf.Toolkit.Base', '', 'WTK'),`
        ('Xceed.Products.Wpf.Toolkit.Base.Themes', '', 'WTK'),`
        ('Xceed.Products.Wpf.Toolkit.ListBox, ''', 'WTK'),`
        ('Xceed.Products.Wpf.Toolkit.ListBox.Themes', '', 'WTK'),`
        ('Xceed.Products.Documents.Libraries.Full', '', 'WDN', 'WBN'),`
        ('Xceed.Products.Zip.Full', 'https://nuget.xceed.com/', 'ZIN'),`
        ('Xceed.Products.Ftp.Full', '', 'FTN'),`
        ('Xceed.Products.RealTimeZip.Full', '', 'ZRT'),`
        ('Xceed.Products.SFtp.Full', '', 'SFT'),`
        ('Xceed.Products.Grid.Full', '', 'GRD'),`
        ('','','')

        # (ProductID, ProductShortName, ProductName, ProductHomePageUrl, ProductPurchaseUrl, ProductTrialKeysWebFormUrl)
[string[][]] $ProductIds = 
        ('','','','',''),`
        ('ZIP','','','','',''),`
        ('SFX','','','','',''),`
        ('BKP','','','','',''),`
        ('WSL','','','','',''),`
        ('FTP','','','','',''),`
        ('SCO','','','','',''),`
        ('BEN','','','','',''),`
        ('CRY','','','','',''),`
        ('FTB','','','','',''),`
        ('ZIN','','Xceed Zip for .NET and .NET Standard','https://xceed.com/en/our-products/product/zip-for-net','',''),`
        ('ABZ','','','',''),`
        ('GRD','','Xceed Grid for WinForms','https://xceed.com/xceed-grid-for-winforms/','',''),`
        ('SCN','','','','',''),`
        ('ZIC','','','','',''),`
        ('SCC','','','','',''),`
        ('SUI','','','','',''),`
        ('SUN','','','','',''),`
        ('FTN','','Xceed FTP for .NET and .NET Standard','https://xceed.com/en/our-products/product/ftp-for-net','',''),`
        ('FTC','','','','',''),`
        ('CHT','','','','',''),`
        ('DWN','','','','',''),`
        ('CHW','','','','',''),`
        ('IVN','','','','',''),`
        ('RDY','','','','',''),`
        ('EDN','','','','',''),`
        ('ZIL','','','','',''),`
        ('TAN','','','','',''),`
        ('DGF','','','','',''),`
        ('DGP','','Xceed DataGrid Pro for WPF','https://www.nuget.org/packages/Xceed.Products.Wpf.Datagrid.Full','https://xceed.com/en/our-products/product/datagrid-for-wpf',''),`
        ('WAN','','','','',''),`
        ('SYN','','','','',''),`
        ('ZIX','','','','',''),`
        ('ZII','','','','',''),`
        ('SFN','','','','',''),`
        ('ZRT','','Xceed Real-Time Zip for .NET and .NET Standard','https://xceed.com/en/our-products/product/real-time-zip-for-net','',''),`
        ('ZRC','','','','',''),`
        ('UPS','','','','',''),`
        ('TDV','','','','',''),`
        ('ZRS','','','','',''),`
        ('XPT','','','','',''),`
        ('OFT','','','','',''),`
        ('GLT','','','','',''),`
        ('MET','','','','',''),`
        ('LET','','','','',''),`
        ('WST','','','','',''),`
        ('DGS','','','','',''),`
        ('LBS','','','','',''),`
        ('ZRP','','','','',''),`
        ('UPP','','','','',''),`
        ('LBW','','','','',''),`
        ('BLD','','','','',''),`
        ('SFT','','Xceed SFtp for .NET and .NET Standard','https://xceed.com/en/our-products/product/sftp-for-net','',''),`
        ('WTK','','Xceed Toolkit Plus for WPF','https://www.nuget.org/packages/Xceed.Products.Wpf.Toolkit.Full','https://xceed.com/en/our-products/product/toolkit-plus-for-wpf',''),`
        ('ZRX','','','','',''),`
        ('ZXA','','','','',''),`
        ('FXA','','','','',''),`
        ('SXA','','','','',''),`
        ('WDN','Xceed_Words_NET','Xceed Words for .NET and .NET5+','https://www.nuget.org/packages/Xceed.Products.Documents.Libraries.Full/','https://xceed.com/en/our-products/product/words-for-net',''),`
        ('PDF','','Xceed PDF Creator for .NET, .NET Standard and .NET5','https://www.nuget.org/packages/Xceed.Products.Documents.Libraries.Full/','https://xceed.com/en/our-products/product/words-for-net',''),`
        ('DGJ','','','','',''),`
        ('WBN','Xceed_Workbooks_NET', 'Xceed Workbooks for .NET and .NET 5+','https://www.nuget.org/packages/Xceed.Products.Documents.Libraries.Full/','https://xceed.com/en/our-products/product/workbooks-for-net',''),
        ('MDN','Xceed_Mail_NET', 'Xceed Mail for .NET and .NET 5+','https://www.nuget.org/packages/Xceed.Products.Documents.Libraries.Full/','https://xceed.com/en/our-products/product/workbooks-for-net','')
  
function shl{
param([System.UInt32] $value, [byte] $nb = 1)

    for([System.Int32] $i=0;$i -lt $nb;$i++)
    {
        $value = $value -band 0x7FFFFFFF
        $value *= 2
    }
    
    return $value
}


function shr{
param([System.UInt32] $value, [byte] $nb = 1)

    for([System.Int32] $i=0;$i -lt $nb;$i++)
    {
        $value = (($value-($value%2)) / 2)
    }
    
    return $value
}

##-------------------------------------------------
## Functions
##-------------------------------------------------
function MapBits{
param([System.Collections.BitArray] $barray, [System.UInt32] $val, [byte[]] $codeBits)

      for( [int] $i = 0; $i -lt ($codeBits.Length - 1); $i++ )
      {
        [int] $x = shl 1 $i
        $ba[ $codeBits[ $i ] ] = ( $val -band $x ) -ne 0
      }
}

function GetBytes{
param([System.Collections.BitArray] $ba)

    [byte[]] $array = New-Object System.Byte[] (9)
    for( [byte] $i = 0; $i -lt $ba.Length; $i++ )
    {
        if($ba[$i])
        {
            [int] $mod = ($i % 8)
            [int] $index = ( $i - $mod ) / 8
            $array[ $index ] = ($array[ $index ]) -bor ([byte]( shr 128 $mod ))
        }
    }
    return $array

}

function CalculateChecksum{
param([System.UInt16[]] $b )

    [System.UInt16] $dw1 = 0
    [System.UInt16] $dw2 = 0

    for([int] $i=0;$i -lt $b.Length;$i++)
    {
        $dw1 += $b[ $i ]
        $dw2 += $dw1
    }    

    ##Reduce to 8 bits
    [System.UInt16] $r1 = ($dw2 -bxor $dw1)
    [byte] $r2 = (shr $r1  8) -bxor ($r1 -band 0x00FF)

    return $r2
}

function GenAlpha {
param([System.Collections.BitArray] $ba)

  [string] $suffix = ''
  [int] $mask = 0x10
  [int] $value = 0
  for( [int] $i = 0; $i -lt $ba.Length;$i++)
  {
    if( $mask -eq 0 )
    {
      $suffix += $AlphaNumLookup[ $value ]
      $value = 0
      $mask = 0x10
    }

    if( $ba[ $i ] )
    {
      $value = $value -bor $mask
    }

    $mask = shr $mask
  }

  $suffix += $AlphaNumLookup[ $value ]

  return $suffix + 'A';
}

function FindPackageTasks {
param([string] $packageId)

    [array] $packageTasks = $null
    
    for( [int] $i = 0; $i -lt $PackagesMap.Length; $i++)
    {
        if($PackagesMap[$i][0] -eq $packageId)
        {
            $packageTasks = $PackagesMap[$i]
            break
        }
    }
    
    return $packageTasks
}

function FindIndex {
param([string] $prodId)
    
    if($prodId -ne '')
    {
      for( [int] $i = 0; $i -lt $ProductIds.Length; $i++)
      {
        if($ProductIds[$i][0] -eq $prodId)
        {
            return $i
        }
      }
    }
    
    return -1
}

function Create {
param([int] $pIndex, [int] $maj, [int] $min, [int[]] $d)

    ## Harcode others values that we dont need to customize.   
    $ba = New-Object System.Collections.BitArray 65
    $ba[6] = $true
    $ba[64] = $true

    [System.DateTime] $date = New-Object -t DateTime -a $d[0],$d[1],$d[2]
    [int] $days = [DateTime]::Today.Subtract($date).Days
    [int] $verNo = ($maj*10) + $min
    [string] $pPrefix = $ProductIds[$pIndex][0]
    [string] $prodId = "$pPrefix$verNo"
    
    MapBits $ba $pIndex $ProductCodeBits
    MapBits $ba $verNo $ProductVersionBits
    MapBits $ba $days $NbDaysBits
    
    [char[]] $a1 = $prodId.ToCharArray()
    [byte[]] $a2 = GetBytes $ba
    [System.UInt16[]] $a = New-Object System.UInt16[] ($a1.Length + $a2.Length)
    
    [System.Array]::Copy($a1,0,$a,0,$a1.Length)
    [System.Array]::Copy($a2,0,$a,$a1.Length,$a2.Length)
        
    [byte] $checksum = CalculateChecksum $a
    
    MapBits $ba $checksum $ChecksumBits

    return $prodId + (GenAlpha $ba)
}

function CreateTrialKeyTextFile {
param([string] $filePath, [string] $prodName, [string] $prodVer, [string] $trialKey, [System.DateTime] $expireDate, [string] $homePage)

    $content = "This is your trial key for """ + $prodName + """ version " + $prodVer + ": " + $trialKey + "`r`n"
    $content = $content + "`r`n"
    $content = $content + "Set the Licenser.LicenseKey property with this license key. See the component documentation topic on 'How to license the component' for details." +  "`r`n"
    $content = $content + "You can find the documentation of all Xceed components online at: https://xceed.com/en/support/documentation-center/" +  "`r`n"
    $content = $content + "`r`n"
    $content = $content + "The key will expire on " + $expireDate.ToString("MMMM dd yyyy") +  ".`r`n"
    $content = $content + "After the trial period is over, you can purchase a subscription to the component here: " + $homePage +  "`r`n"
        
    $content | Out-File -FilePath $filePath
}

function CreateTrialKeyHTMLFile {
param([string] $filePath, [string] $prodName, [string] $prodVer, [string] $trialKey, [System.DateTime] $expireDate, [string] $homePage)

    $content = "<!doctype html>`r`n"
    $content = $content + "<html lang=""en"">`r`n"
    $content = $content + "<head>`r`n"
    $content = $content + "    <meta charset=""utf-8"">`r`n"
    $content = $content + "    <meta name=""viewport"" content=""width=device-width, initial-scale=1"">`r`n"
    $content = $content + "    <title>Trial key - $prodName</title>`r`n"
    $content = $content + "    <style>`r`n"
    $content = $content + "        body { margin: 0; padding: 0; }`r`n"
    $content = $content + "        a { color: #ff671b; }`r`n"
    $content = $content + "        .header { height: 4px; background-color: #f9bf3a; }`r`n"
    $content = $content + "        .text { font-family: Verdana, Arial, Helvetica, sans-serif; font-size: smaller; color: black; margin-left: 35px; margin-right: 20%; }`r`n"
    $content = $content + "        .highlight { font-weight: bolder; }`r`n"
    $content = $content + "        .reference { font-style: italic; }`r`n"
    $content = $content + "    </style>`r`n"
    $content = $content + "</head>`r`n"
    $content = $content + "<body>`r`n"
    $content = $content + "    <div class=""header""></div>`r`n"
    $content = $content + "    <p class=""text"">This is your trial key for <span class=""highlight"">$prodName</span> version <span class=""highlight"">$prodVer</span>: <span class=""highlight"">$trialKey</span></p>`r`n"
    $content = $content + "    <p class=""text"">`r`n"
    $content = $content + "        Set the <code>Licenser.LicenseKey</code> property with this license key. See the component documentation topic on <span class=""reference"">How to license the component</span> for details.<br>`r`n"
    $content = $content + "        You can find the documentation of all Xceed components online at: <a target=""_blank"" href=""https://xceed.com/en/support/documentation-center/"">https://xceed.com/en/support/documentation-center/</a>`r`n"
    $content = $content + "    </p>`r`n"
    $content = $content + "    <p class=""text"">`r`n"
    $content = $content + "        The key will expire on <span class=""highlight"">" + $expireDate.ToString("MMMM dd yyyy") + "</span>.<br>`r`n"
    $content = $content + "        After the trial period is over, you can purchase a subscription to the component here: <a target=""_blank"" href=""$homePage"">$homePage</a>`r`n"
    $content = $content + "    </p>`r`n"
    $content = $content + "</body>`r`n"
    $content = $content + "</html>`r`n"
        
    $content | Out-File -FilePath $filePath
}

function TestAndCreate {
param([string] $path)

    if(!(Test-Path $path))
    {
        $dump = New-Item $path
    }
}

function TestAndCreateDirectory {
param([string] $path)

    if(!(Test-Path $path))
    {
        $dump = New-Item -ItemType directory -Path $path
    }
}

function Setup {
param([int] $pIndex, [int] $major, [int] $minor)

    try
    {
        if( $pIndex -lt 0 )
        {
            Write-Host "Failed to find the product."
            return $null, $null
        }        

        if( ( $major -lt 0 ) -or ( $major -gt 9 ) -or ( $minor -lt 0 ) -or ( $minor -gt 9 ) )
        {
            Write-Host "Failed to generate a license key."
            return $null, $null
        }

        [int[]] $d = $null
        if( ( $pIndex -eq 58 ) -or ( $pIndex -eq 59 ) -or ( $pIndex -eq 61 ) )
        {
            $d = $td2
        }
        else
        {
            $d = $td1
        }
 
        ## Create the trial key
        [string] $k = Create $pIndex $major $minor $d

        ## Begin - Write trial key to text file
        $trialPath = "C:\Xceed Trial Keys"
        TestAndCreateDirectory $trialPath

        $pName = $ProductIds[$pIndex][2]
        $pNameShort = $ProductIds[$pIndex][1]
        $pHomeUrl = $ProductIds[$pIndex][3]
        $pPurchaseUrl = $ProductIds[$pIndex][4]
        [string] $prodVer = "$major.$minor"
        [DateTime] $e = [DateTime]::Today.AddDays($d[3])

        if( $pNameShort -ne '' )
        {
          $pNameShort = "_" + $pNameShort
        }
        
        $trialTextFilePath = Join-Path -Path $trialPath -ChildPath "$($package.Id)$($pNameShort)_$($prodVer)_Key.txt"
        $trialHTMLFilePath = Join-Path -Path $trialPath -ChildPath "$($package.Id)$($pNameShort)_$($prodVer)_Key.html"

        CreateTrialKeyTextFile $trialTextFilePath $pName $prodVer $k $e $pPurchaseUrl
        CreateTrialKeyHTMLFile $trialHTMLFilePath $pName $prodVer $k $e $pPurchaseUrl
        ## End - Write trial key to text/html file

        ## Write trial key to registry
        [string] $prodPath = $licensesPath + '\' + $ProductIds[$pIndex][0]
       
        TestAndCreate $basePath
        TestAndCreate $licensesPath
        TestAndCreate $prodPath

        [Microsoft.Win32.RegistryKey] $path = Get-Item $prodPath
        if($null -eq $path.GetValue($prodVer, $null))
        {
            Set-ItemProperty -Path $prodPath -Name $prodVer -Value $k
        }

        return $pHomeUrl, $trialHTMLFilePath
    }
    catch
    {
    }

    return $null, $null
}


function ParseVersion {
param([string] $version)

    try
    {
        return [System.Version]::Parse($version)
    }
    catch
    {
        return $null
    }
}

function GetVSVersion {

    try
    {
        return ParseVersion $dte.Version
    }
    catch
    {
        return $null
    }
}

function GetPackageVersion {

    [System.Version] $vs = GetVSVersion
    [System.Version] $retval = $null

    if($null -ne $vs)
    {
        ## Visual Studio 2015 and later
        if($vs.Major -ge 14)
        {
            $retval = ParseVersion $package.Version
        }
        ## Visual Studio 2013 and earlier
        elseif($vs.Major -gt 0)
        {
            $retval = $package.Version.Version
        }
    }

    return $retval
}

function ExtractPackageVersion {

    [System.Version] $version = GetPackageVersion
    
    if($null -ne $version)
    {
        return $version.Major, $version.Minor
    }
    else
    {
        return -1, -1
    }
}

##-------------------------------------------------
## Entry Point (Main)
##-------------------------------------------------

# $dte = @{}
# $dte.Version = New-Object -TypeName System.Version -ArgumentList "14.0.0"

# $package = @{}
# $package.Id = 'Xceed.Products.Documents.Libraries.Full'
# $package.Version = New-Object -TypeName System.Version -ArgumentList "2.0.0"

# [string] $toolsPath = 'D:\Components\NET40\DocumentLibraries\Dev\Trunk\Installer\DocumentLibraries\NuGet\Temp'

[System.Collections.Hashtable] $assemblyProperties = $null
[string] $assemblyPropertiesFile = Join-Path -Path $toolsPath -ChildPath AssemblyProperties.txt
if( Test-Path -Path $assemblyPropertiesFile -PathType Leaf )
{
    [string] $assemblyPropertiesFileData = Get-Content -Raw -Path $assemblyPropertiesFile
    $assemblyProperties = ConvertFrom-StringData -StringData $assemblyPropertiesFileData
}

[array] $tasks = FindPackageTasks $package.Id

if($tasks -ne $null)
{
    for( [int] $i = 2; $i -lt $tasks.Length; $i++)
    {
        [string] $productCode = $tasks[$i]
        [int] $major = -1
        [int] $minor = -1

        [int] $pIndex = FindIndex $productCode

        [string] $assemblyName = $ProductIds[$pIndex][1]
        if( $assemblyName -ne '' )
        {
            if( ( $assemblyProperties -ne $null ) -and $assemblyProperties.ContainsKey( $assemblyName + '_Major' ) -and $assemblyProperties.ContainsKey( $assemblyName + '_Minor' ) )
            {
                $major = $assemblyProperties[ $assemblyName + '_Major' ]
                $minor = $assemblyProperties[ $assemblyName + '_Minor' ]
            }
            else
            {
                Write-Host "Failed to find assembly properties for '" + $assemblyName + "'."
                return
            }
        }
        else
        {
            $major, $minor = ExtractPackageVersion
        }
	
        $pHomeUrl, $pKeyFile = Setup $pIndex $major $minor
        
        if( ( $pHomeUrl -ne $null ) -and ( $pHomeUrl.Length -gt 0 ) )
        {
          [void] $project.DTE.ItemOperations.Navigate($pHomeUrl, 1)
        }

        if( ( $pKeyFile -ne $null ) -and ( $pKeyFile.Length -gt 0 ) )
        {
            [void] $project.DTE.ItemOperations.Navigate($pKeyFile, 1)
        }

	$pFormUrl = $ProductIds[$pIndex][5]
        if( ( $pFormUrl -ne $null ) -and ( $pFormUrl.Length -gt 0 ) )
        {
            [void] $project.DTE.ItemOperations.Navigate($pFormUrl, 1)
        }
    }

    # When the package contains a key form, display the form to fill in order to receive an email with the package's products trial keys.
    [string] $packageKeyForm = $tasks[1]
    if( $packageKeyForm -ne '' )
    {
      # [void] $project.DTE.ItemOperations.Navigate($packageKeyForm, 1)
	  #Start-Process $packageKeyForm

	  #Get Default browser
      $DefaultSettingPath = 'HKCU:\SOFTWARE\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice'
      $DefaultBrowserName = (Get-Item $DefaultSettingPath | Get-ItemProperty).ProgId
    
      #Handle for Edge
      ##edge will no open with the specified shell open command in the HKCR.
      if($DefaultBrowserName -eq 'AppXq0fevzme2pys62n3e0fbqa7peapykr8v')
      {
          #Open url in edge
          start Microsoft-edge:$packageKeyForm
      }
      else
      {
          try
          {
            #Create PSDrive to HKEY_CLASSES_ROOT
            $null = New-PSDrive -PSProvider registry -Root 'HKEY_CLASSES_ROOT' -Name 'HKCR'
            #Get the default browser executable command/path
            $DefaultBrowserOpenCommand = (Get-Item "HKCR:\$DefaultBrowserName\shell\open\command" | Get-ItemProperty).'(default)'
            $DefaultBrowserPath = [regex]::Match($DefaultBrowserOpenCommand,'\".+?\"')
            #Open URL in browser
            Start-Process -FilePath $DefaultBrowserPath ('--new-window',  $packageKeyForm)
          }
          catch
          {
              Throw $_.Exception
          }
          finally
          {
              #Clean up PSDrive for 'HKEY_CLASSES_ROOT
              Remove-PSDrive -Name 'HKCR'
          }
      }
    }
}