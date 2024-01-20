# Leashmore

Leashmore is an open-source "Operations Management Software" distributed under the MIT License. It employs military-grade encryption alongside sophisticated integrity verification to ensure data security.
You can freely modify and distribute copies and any modifications of the
Leashmore.
You can use Leashmore in commercial applications.

SourceForge page can be found at https://sourceforge.net/projects/leashmore/

## Features
- AES-256 in CBC-mode (military-grade encryption)
- HMAC-SHA256 (integrity verification)
- SQLite Relational Database Management System
- Unicode Support
- Password Hashing + Hash Encryption

## Database rules
- A worker can have no specialization or only one specialization.
- A specialization may be possessed by no workers, one worker, or several workers.
- A gig may belong to no worker or belong to one or several workers, but no worker can have the same gig twice.
- A worker can have no gigs, one gig, or several gigs.
- A payment may only belong to one worker.

## Requirements
- Windows 10 or compatible OS
- [.NET 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

## User Guide
Launch the "Leashmore.exe" file from the "software/bin/Debug/net6.0-windows/"

When launching Leashmore for the first time, you will be prompted to set a password to unlock it. Please note that the cryptographic keys used to decrypt data are derived directly from your chosen password. As a result, you will not be able to change your password once it has been set. Additionally, if you lose your password, the data stored in the database will be inaccessible, as the Leashmore will be unable to decrypt it.

![image text](https://github.com/Northstrix/Leashmore/blob/main/V1.0/Pictures/Set%20Password.png)

As you enter the password you would like to use, press the "Continue" button. After pressing that button, you should see the "Password Set Successfully" message.

![image text](https://github.com/Northstrix/Leashmore/blob/main/V1.0/Pictures/Password%20Set%20Successfully%20Message.png)

Once you've successfully unlocked Leashmore, you will be able to see the main form as depicted below. If you wish to add any specializations, you can do so by selecting "Specialization" and then clicking on "Add". However, if you would like to skip that part, it can be done later.

![image text](https://github.com/Northstrix/Leashmore/blob/main/V1.0/Pictures/Leashmore%20Main%20Form.png)

To begin adding workers, click on the "Add Worker" button and fill out the necessary information in the pop-up window. Once you have filled out the required fields, click on the "Add" button in the same window to add the worker's record to the database.

![image text](https://github.com/Northstrix/Leashmore/blob/main/V1.0/Pictures/Add%20Worker%20Form.png)

If you haven't selected a specialization, the Leashmore software will show you a warning depicted below. Please note that the presence or absence of a specialization does not affect the financial aspect of the software. Therefore, if you choose not to assign a specialization to your workers, it will not affect their payments.

![image text](https://github.com/Northstrix/Leashmore/blob/main/V1.0/Pictures/Specialization%20Isn't%20Selected%20Warning.png)

Once you have added a worker's record, a confirmation message "Record Added Successfully!" will be displayed.

![image text](https://github.com/Northstrix/Leashmore/blob/main/V1.0/Pictures/Record%20Added%20Successfully%20Message.png)

Once you close this message window, you will be able to see the updated worker's table. To demonstrate the software's Unicode support, I have added multiple workers to the table.

![image text](https://github.com/Northstrix/Leashmore/blob/main/V1.0/Pictures/Leashmore%20Main%20Form%20With%20Records.png)

To add gigs in Leashmore, click on "Gig" and then select "Add." Leashmore enables you to add two types of gigs: result-paid gigs and hourly-paid gigs. For result-paid gigs, you only need to enter the amount that the worker will be paid for completing the entire gig. For hourly-paid gigs, you need to enter the hourly rate (pay) and the number of hours that the worker will be working.

