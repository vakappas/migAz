Hello Azure expert!

This repo is the home for the MigAz. MigAz intent is to help you on Azure Resource Manager (ARM) deployments and migrations.

The base principal of MigAz tools is to work on top of ARM templates. This creates the foundations for you to create, change and migrate deployments with the power and flexibility of ARM templates deployment model. This is true if you are now starting to work with ARM templates and want to learn, or you already have advanced knowledge but need a tool to make you even more productive.

All versions (except ASM Native) work the same way. Connects to the source environment, allows you to select the objects you need to export and create an ARM template from it. With this ARM template you can now make changes, clone or migrate an environment.
Have a look to each ASM, ARM and AWS versions and see where it helps you.

Give us feedback so we continue to improve this tools and, specially, help us by submitting your own updates. Remember this are community tools that depend deeply on your feedback and code contributions.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Pre-requisites
1. Windows 8 or Windows Server 2012, or later
2. .Net Framework 4.0 or higher
3. Latest [Azure PowerShell Module](https://azure.microsoft.com/en-us/documentation/articles/powershell-install-configure)
4. Login credentials at source Azure subscription
5. Login credentials at destination Azure subscription

## Get it
Download from the [Release](Release) folder to obtain the latest MigAz v2 release (currently ASM to ARM and ARM to ARM Migration).  Note that Migaz AWS to ARM is not yet integrated into the new v2 project baseline and remains available via the AWS release folders ([AWS Release](aws/release)).

## Instructions
[Azure ASM to Azure ARM Migration](asm) - MigAz ASM to ARM will help you on your ASM to ARM migrations. Allows you to create an ARM template out of an Azure IaaS ASM deployment. It also provides an automation script for blobs copy.

[Azure ARM to Azure ARM Migration](arm) - MigAz ARM to ARM version allows you to create an ARM template out of an Azure IaaS ARM deployment. It also provides an automation script for blobs copy.

[AWS to Azure ARM Migration](aws) - MigAz AWS version will help you on your AWS to Azure migrations. MigAz AWS version allows you to create an ARM template out of an AWS EC2 deployment.

## Troubleshooting
The detailed logs and output of the REST API are captured in the location %USERPROFILE%\appdata\Local with the file name MigAz-&lt;YYYYMMDD&gt;.log and MigAz-XML-&lt;YYYYMMDD&gt;.log.
In case of any issues during the deployment of the export.JSON you need to troubleshoot the template properties and fix the invalid entries. Report any issue on the tool site.


## Release Notes
### v2.2.14.0
 - Added Storage Account Resource Group Name into copyblobdetails.json and removed as BlobCopy.ps1 parameter to resolve use of existing storage accounts in other Resource Groups(s).
 - Incrased maximum disk size from 1023 to 4095 per Azure support for 4 TB drives.

### v2.2.12.0
 - Storage Account property selection now only allows selection from existing Storage Account(s) in "same" target location (same Azure Data Center as the Target Resource Group is targeted for)
 - Generates storageaccounts.json
 - Output Stream tab reordering (based on order generated)
 - Added support to recognize if Azure ARM VM Disk(s) are Encrypted, present migration error message indicating that MigAz does not contain support to migrated encrypted disks.

### v2.2.3.0
- Only exports copyblobdetails.json if there are disks that are being copied.
- Reintroduced ASM to ARM conversion of Load Balancers and Public IP objects, utilizing "target" objects for export.

### v2.2.1.0
- Fixed ASM Virtual Machine constructor with null Subnet
- Added Control resizing to allow larger (height) TreeView controls on form

### v2.2.0.0
- Consolidated stand alone MigAz ARM to ARM version into new MigAz v2.2.0.0 release.  Discontinued MigAz ARM to ARM stand alone version. 
- Open need to validate ASM to ARM Load Balancer and Public IP generation (proceeding with release to resolve prior issues).
- Next steps to author Managed Disk integration, then consolidation of MigAz AWS to ARM.

### v2.1.0.0
- Added retry logic with back off timer to Azure REST API calls on 500 HTTP Error response
- Fixed ARM Disk Target Storage Account drop down binding continually defaulting to first ARM Storage Account
- Added Tenant Selection to Target Subscription selection dialog
- Updated and deployed TenantId in HTML Documentation

### v2.0.0.0
- Multiple enhancements and bug fixes
- MigAz ASM to ARM updated and released as MigAz v2.0.0.0
- MigAz ARM to ARM and MigAz AWS to ARM remain in their existing v1.x Visual Studio solutions with upcoming plans to incorporate them into the v2.x single solution baseline.

### v1.5.0.1
- Correction to ARM API URL Endpoints

### v1.5.0.0
- Upgrade projects to .NET Framework 4.6.2, per [End of Support for Framework 4.5.x](https://blogs.msdn.microsoft.com/dotnet/2015/12/09/support-ending-for-the-net-framework-4-4-5-and-4-5-1/)
- Updated project package reference and code for Microsoft.IdentityModel.Clients.ActiveDirectory from 2.20.0.0 to 3.13.8.0
- Moved selection of Azure Environment out from Option Dialog, enabling streamlined selection prior to login as well as ongoing awareness of selected Azure Environment during Asm To Arm process
- Auto selects (binds to) Azure Subscription if the authenticated Azure ASM Account only has access to a single subscription
- Auto logout (upon successful login) from Azure AD Account if the authenticated account does not have any subscriptions present in the identified Azure Environment
- Introduced TreeView for viewing and selecting ASM Resources (Virtual Networks, Storage Accounts, Cloud Services, Virtual Machines) for export, creating clearer visibility of resources in subscriptions that have ASM resources deployed across multiple Azure Locations
- Introduced Treeview for viewing of Target ARM Resource output, increasing visiblity prior to actual export
- Introduced Azure Resource Object Detail property explorer pane, increasing object visiblity prior to actual export as well as allowing for object revision prior to export
- All ASM / XmlNode logic centralized into Asm classes for reusable/centralized code base of all XMLNode parsing logic (externalized from actual template generation process)
- Updated Unit Tests for successful pass with FakeRetriever overrides against new to AzureRetriever structure
- Added Query of Azure Locations available to subscription, used to populate available Location ComboBox
- Added population of target location on pre export dialog to populate locations available to target subscription (inclusive of rare migration condition across Azure Environments)
- Resolved bug in which only the first Address Prefix of a VPN Local Site that contains multiple Address Prefixes was exported
- Added Azure REST API calls to query existing ARM based resources (Virtual Networks, Storage Accounts and Storage Account Keys) to aid in migration scenarios of ASM Virtual Machines targeting into existing ARM Virtual Networks and Storage Accounts
- Ensured that GatewaySubnet is not populated in ASM/ARM Subnet as a user deployable Target Subnet
- Introduced separate Export Parameter dialog with previous inputs for output path and Resource Group, added Target Azure Subscription and Target Azure Location
- Removed "Next Steps" Tab from Export Result dialog, as Azure Location selection was moved for input prior to export occurring.  Show Deployment button moved to status screen
- Implemented initial pre-export check, requiring Target Resource name selection
- Update blank VM export to target Windows Server 2016 instead of Windows Server 2012, per image availablity across all Azure Regions
- Moved output messages generated by MigAz from export completed form into HTML deployment instructions
- Revised use of Azure Environment Names (removing spaces) to be consistent with Environment Names used across other platforms (i.e. PowerShell)

### v1.4.10.0
- Fix script samples in generated HTML instructions

### v1.4.9.0
- Performance improvements, minor fixes to generated resource names

### v1.4.8.0
- Fix error when exporting a VM that is in a VNET with no subnets defined

### v1.4.7.0
- New dialog after exporting template that generates instructions on how to deploy to ARM.

### v1.4.6.0
- Support for VNETs attached to ExpressRoute circuts
- Fix unit tests

### v1.4.5.0
- Add Auto Select Dependencies option
- Add Save Selection feature
- Fix error when LoadBalancedEndpointSetName have spaces. Remove spaces
- Correct behavior when new storage account name have more than 24 chars

### v1.4.4.0
- Add requirement for Azure.Storage and AzureRM.Storage modules to be 2.0.1
- Add to BloblCopy.ps1 the option to cancel the blobs copy jobs
- Remove clear screen command from BlobCopy.ps1 bor better results reading

### v1.4.3.0
- Add ability to export resources deployed on Affinity Groups
- Prevent exception when VPN Gateway connection shared key is not defined

### v1.4.2.1
- . Fix reported issue when logging directory does not exist

### v1.4.2.0
- Process Cloud Services Reserved IPs to Static Public IPs
- Add tool version and subscription offer categories to telemetry
- Improvements on typical exceptions handling
- Minor UI updates

### v1.4.1.1
- Correcting case where VM disk URL does not end with ".vhd". Not an usual situation, but possible.
- Add logging when retrieving objects from xml cache
- Correcting TabIndex for better usability
- Resetting xml cache when re-login