# Hiwell.AddressBook
Hiwell address book case study

Live demo is [here](http://hiwelladdressbookapi-test.us-east-2.elasticbeanstalk.com/)

- Demo is hosted on an Aws elastic beanstalk (64-Bit Linux instance).
- PostgreSql DB is managed by AWS (RDS).
- Aws code pipeline infrastructure is used for CI/CD.
- Tests run after each push to master.
- Tests run against a sqlite database. This database can be used for local debugging.
