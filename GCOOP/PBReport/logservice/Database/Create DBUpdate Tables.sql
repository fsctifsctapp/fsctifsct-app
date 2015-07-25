
/*
ACTION is CREATE Table dbupdatehistory
*/
CREATE TABLE dbupdatehistory (
       audit_id             NUMBER(10,0) NOT NULL,
       modified_id          CHAR(15) NULL,
       modified_date        DATE NULL,
       oldvalue_id          NUMBER(10,0) NULL,
       newvalue_id          NUMBER(10,0) NULL
);

CREATE UNIQUE INDEX XPKdbupdatehistory ON dbupdatehistory
(
       audit_id
);

ALTER TABLE dbupdatehistory
       ADD  ( PRIMARY KEY (audit_id) ) ;


/*
ACTION is CREATE Table dbupdatevalues
*/
CREATE TABLE dbupdatevalues (
       value_id             NUMBER(10,0) NOT NULL,
       seq_no               NUMBER(10,0) NOT NULL,
       xml_value            VARCHAR2(3096) NULL
);

CREATE UNIQUE INDEX XPKdbupdatevalues ON dbupdatevalues
(
       value_id,
       seq_no
);

ALTER TABLE dbupdatevalues
       ADD  ( PRIMARY KEY (value_id, seq_no) ) ;





      
      /*
      ACTION is CREATE Table dbconst
      */

CREATE TABLE dbconst (
       const_name           CHAR(10) NOT NULL,
       const_value          VARCHAR2(3096) NULL
);

CREATE UNIQUE INDEX XPKdbconst ON dbconst
(
       const_name
);


ALTER TABLE dbconst
       ADD  ( PRIMARY KEY (const_name) ) ;


