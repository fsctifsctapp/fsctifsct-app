      
      /*
      ACTION is CREATE Table WEBREPORTDETAIL
      */

CREATE TABLE WEBREPORTDETAIL (
       group_id             VARCHAR2(40) NOT NULL,
       report_id            VARCHAR2(40) NOT NULL,
       report_name          VARCHAR2(100) NULL,
       report_dwobject      VARCHAR2(70) NULL,
       report_criobject     VARCHAR2(70) NULL,
       report_creobject     VARCHAR2(70) NULL,
       report_comment       VARCHAR2(100) NULL,
       used_flag            NUMBER(1) NULL
);


ALTER TABLE WEBREPORTDETAIL
       ADD  ( PRIMARY KEY (group_id, report_id) ) ;

      
      /*
      ACTION is CREATE Table WEBREPORTGROUP
      */

CREATE TABLE WEBREPORTGROUP (
       group_id             VARCHAR2(40) NOT NULL,
       group_name           VARCHAR2(60) NULL,
       ref_group_id         VARCHAR2(40) NULL,
       APPLICATION          VARCHAR2(20) NULL
);


ALTER TABLE WEBREPORTGROUP
       ADD  ( PRIMARY KEY (group_id) ) ;


ALTER TABLE WEBREPORTDETAIL
       ADD  ( FOREIGN KEY (group_id)
                             REFERENCES WEBREPORTGROUP ) ;
