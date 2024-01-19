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
