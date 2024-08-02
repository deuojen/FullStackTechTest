# FourteenFish - Full Stack Technical Assessment

# Overview

This app that stores information about medical professionals, such as doctors. Written in C# .NET app to build upon, along with seed data for a MySQL database.
To seed the database use seed.sql file then execute 1-migrate.sql file for new feature implemantation 

## Feature 1: Import data

Implement an import feature to parse, validate, and store new entries. Sample json file exists in root directory.

### User stories

- As an admin, I want to import users from an external source so that I can migrate new users into our app.

### Changes

- No database change
- Insert new route/controller
- Add import data repository for check duplicate data and insert or ignore.


### Acceptance criteria:

- All doctors have a unique 7-digit GMC number.
- Given that admins may accidentally import the same data more than once, ensure new entries are only imported once.

## Feature 2: Add specialities

A requirement to record each doctor's medical specialities. Example specialities include:

- Anaesthetics
- Cardiology
- Dermatology
- Emergency Medicine
- General Practice (GP)
- Neurology
- Obstetrics and Gynaecology
- Ophthalmology
- Orthopaedic Surgery
- Psychiatry

### User stories

- As an admin, I want to add and remove medical specialities so I can manage them in the system.
- As an admin, I want to associate an existing speciality with a doctor's profile so that we can record what specialities they can perform.
- As an admin, I want to see the specialities associated with a doctor when viewing their details page so that we know what specialities they can perform.

### Acceptance criteria:

- Given that a doctor may have more than one speciality, admins should be able to associate a doctor with more than one.
- Given that a doctor may stop practising a speciality, admins should be able to remove them for a user.
- Given that an admin may make a mistake when adding a speciality name, updating the speciality should update it for all doctors associated with it.

### Changes

- Database change for add 2 new tables and seed data
- Insert new route/controller for specialties
- Add speciality assignment section under person edit.
