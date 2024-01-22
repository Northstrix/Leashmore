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
- Collision Avoidance (Ensures that no two records in the same table have the same Rec_id)

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

When launching Leashmore for the first time, you will be prompted to set a password to unlock it. Please note that the cryptographic keys used to decrypt data are derived directly from your chosen password. As a result, you will not be able to change your password once it has been set. Additionally, if you lose your password, the data stored in the database will be inaccessible, as the Leashmore won't be able to decrypt it.

![image text](https://github.com/Northstrix/Leashmore/blob/main/V1.0/Pictures/Set%20Password.png)

As you enter the password you would like to use, press the "Continue" button. After pressing that button, you should see the "Password Set Successfully" message.

![image text](https://github.com/Northstrix/Leashmore/blob/main/V1.0/Pictures/Password%20Set%20Successfully%20Message.png)

Once you've successfully unlocked Leashmore, you will be able to see the main form as depicted below. If you wish to add any specializations, you can do so by selecting "Specialization" and then clicking on "Add". However, if you would like to skip that part, it can be done later.

![image text](https://github.com/Northstrix/Leashmore/blob/main/V1.0/Pictures/Leashmore%20Main%20Form.png)

To begin adding workers, click on the "Add Worker" button and fill out the necessary information in the pop-up window. Once you have filled out the required fields, click on the "Add" button in the same window to add the worker's record to the database.

![image text](https://github.com/Northstrix/Leashmore/blob/main/V1.0/Pictures/Add%20Worker%20Form.png)

If you haven't selected a specialization, the Leashmore software will show you a warning depicted below. Please note that the presence or absence of a specialization does not affect the financial aspect of the software. Therefore, if you choose not to assign a specialization to your workers, it will not affect their payments.

![image text](https://github.com/Northstrix/Leashmore/blob/main/V1.0/Pictures/Specialization%20Isn't%20Selected%20Warning.png)

A confirmation message "Record Added Successfully!" will be displayed after the record is added to the database

![image text](https://github.com/Northstrix/Leashmore/blob/main/V1.0/Pictures/Record%20Added%20Successfully%20Message.png)

Once you close this message window, you will be able to see the updated worker's table. To demonstrate the software's Unicode support, I have added multiple workers to the table.

![image text](https://github.com/Northstrix/Leashmore/blob/main/V1.0/Pictures/Leashmore%20Main%20Form%20With%20Records.png)

This is how the records look like in an encrypted form within the database.

![image text](https://github.com/Northstrix/Leashmore/blob/main/V1.0/Pictures/Records%20in%20the%20database.png)

To add gigs in Leashmore, click on "Gig" and then select "Add." Leashmore enables you to add two types of gigs: result-paid gigs and hourly-paid gigs. For result-paid gigs, you only need to enter the amount that the worker will be paid for completing the entire gig. For hourly-paid gigs, you need to enter the hourly rate (pay) and the number of hours that the worker will be working.

![image text](https://github.com/Northstrix/Leashmore/blob/main/V1.0/Pictures/Select%20Gig%20Type.png)

When you've decided what type of gig you want to add, check the corresponding radio button and hit "Continue."

Enter the gig title, start and end dates, payment for the gig, and gig description. Click the "Add" button to record the gig in the database. By the way, you don't have to worry about using commas for decimal points because Leashmore automatically replaces commas with decimal points for numeric fields when it adds records to the database.

![image text](https://github.com/Northstrix/Leashmore/blob/main/V1.0/Pictures/Add%20Result%20Paid%20Gig%20Form.png)

A confirmation message "Record Added Successfully!" will be displayed after the record is added to the database

![image text](https://github.com/Northstrix/Leashmore/blob/main/V1.0/Pictures/Record%20Added%20Successfully%20Message.png)

After adding a gig to the database, you can finally assign it to one or several workers. To assign a gig to a worker, select the worker's name, click on the "Add Gig to Worker" button, and choose the gig type. Then, click on the "Continue" button and select the gig you want to assign to the worker from the pop-up window. Finally, click on the "Add" button to complete the assignment process.
When you assign a gig to the worker, they become entitled to the amount of money specified in the gig, so if you assign a $100 gig to three workers, each of them would be entitled to $100.

![image text](https://github.com/Northstrix/Leashmore/blob/main/V1.0/Pictures/Leashmore%20Main%20Form%20With%20Selected%20Worker.png)

![image text](https://github.com/Northstrix/Leashmore/blob/main/V1.0/Pictures/Select%20Gig%20Type.png)

![image text](https://github.com/Northstrix/Leashmore/blob/main/V1.0/Pictures/Select%20Gig%20To%20Assign%20To%20Worker.png)

![image text](https://github.com/Northstrix/Leashmore/blob/main/V1.0/Pictures/Result%20Paid%20Gig%20Added%20to%20Worker%20Successfully%20Message.png)

![image text](https://github.com/Northstrix/Leashmore/blob/main/V1.0/Pictures/Leashmore%20Main%20Form%20With%20Selected%20Worker%20After%20Adding%20Gig.png)

Once you assign a gig to a worker, they are entitled to the payment specified for that gig. To pay the worker, select their name, click on the "Pay to Worker" button, enter the payment amount, and click on the "Pay" button.

Also, you can change the currency symbol displayed by editing the "currency.txt" file found in the same folder as the "Leashmore.exe" file after setting the master password.

You can explore other features of Leashmore on your own.

## Errors
Leashmore can give you the following errors related to the data decryption:

![image text](https://github.com/Northstrix/Leashmore/blob/main/V1.0.1/Pictures/Failed%20to%20Verify%20Integrity%2CAuthenticity%20of%20a%20Ciphertext.png)

![image text](https://github.com/Northstrix/Leashmore/blob/main/V1.0.1/Pictures/Failed%20to%20Decrypt%20Ciphertext%20Message.png)
