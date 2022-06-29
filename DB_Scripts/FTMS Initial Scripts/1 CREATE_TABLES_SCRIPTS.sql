CREATE SCHEMA ftms;
Drop table if exists ftms.ftms_file cascade;
Drop table if exists ftms.ftms_census cascade;
Drop table if exists ftms.lkp_file_condition cascade;
Drop table if exists ftms.ftms_file_request cascade;
Drop table if exists ftms.ftms_locations cascade;
Drop table if exists ftms.request_details cascade;
Drop table if exists ftms.ftms_transaction cascade;
Drop table if exists ftms.ftms_setting cascade;
Drop table if exists ftms.ftms_sticker cascade;

CREATE table ftms.ftms_file
( 
 file_id integer,
 file_number varchar(90) not null,
 owner_name varchar(300) not null,
 cofo_exist boolean not null,
 rofo_exist boolean not null,
 phone_number varchar(255),
 app_date timestamp without time zone,
 remark text,
 register_number varchar(20) not null,
 rofo_date timestamp without time zone,
 commencement_date timestamp without time zone,
 file_alias varchar(90),
 lga_code varchar(50),
 rack_number varchar(20) not null,
 current_transaction integer, 
 PRIMARY KEY (file_id)
);
CREATE SEQUENCE ftms.ftms_file_seq owned by ftms.ftms_file.file_id;
ALTER TABLE ftms.ftms_file ALTER COLUMN file_id SET DEFAULT nextval('ftms.ftms_file_seq');


CREATE TABLE ftms.ftms_census
(
  census_id integer,
  census_date timestamp without time zone not null,
  location_code varchar(20) not null,
  file_id integer not null,
  logged_user varchar (90) not null,
  pc_name varchar(90) not null,
  PRIMARY KEY(census_id)
);
CREATE SEQUENCE ftms.ftms_census_seq owned by ftms.ftms_census.census_id;
ALTER TABLE ftms.ftms_census ALTER COLUMN census_id SET DEFAULT nextval('ftms.ftms_census_seq');


CREATE TABLE ftms.lkp_file_condition
(
  mr_code varchar(20),
  description character varying(255) not null,
  active boolean not null,
  positions integer not null,
  PRIMARY KEY(mr_code)
);

CREATE TABLE ftms.ftms_file_request
(
  request_id integer,
  requestor_name varchar(255) not null,
  request_purpose varchar(500) not null,
  request_date timestamp without time zone NOT NULL,
  requestor_pcname varchar(90) NOT NULL,
  requestor_logged_user varchar(90) NOT NULL,
  PRIMARY KEY(request_id)
);
CREATE SEQUENCE ftms.ftms_file_request_seq owned by ftms.ftms_file_request.request_id;
ALTER TABLE ftms.ftms_file_request ALTER COLUMN request_id SET DEFAULT nextval('ftms.ftms_file_request_seq');


CREATE TABLE ftms.ftms_locations
(
  mr_code varchar(20) PRIMARY KEY,
  description varchar(255) not null,
  active boolean not null,
  positions integer not null
);

CREATE TABLE ftms.request_details
(
  request_id integer not null,
  file_id integer not null,
  note text,
  PRIMARY KEY(request_id,file_id)
);


CREATE TABLE ftms.ftms_transaction
(
  transaction_id integer,
  file_id integer not null,
  from_location varchar(20) not null,
  to_location varchar(20) not null,
  transaction_date timestamp without time zone not null,
  file_condition varchar(20) not null,
  number_page integer NOT NULL,
  logged_user varchar(90),
  pc_name varchar(90),
  tracking_remark varchar(1000),
  PRIMARY KEY(transaction_id)
);

CREATE SEQUENCE ftms.ftms_transaction_seq owned by ftms.ftms_transaction.transaction_id;
ALTER TABLE ftms.ftms_transaction ALTER COLUMN transaction_id SET DEFAULT nextval('ftms.ftms_transaction_seq');


CREATE TABLE ftms.ftms_setting(  
	mr_code character varying (20) PRIMARY KEY,  
	value character varying (200) NOT NULL,  
	active boolean NOT NULL,
	description character varying (255)
);

CREATE TABLE ftms.ftms_sticker 
(
  sticker_id integer not null,
  file_id integer not null,
  qr_full_text character varying(255) not null,
  created_date timestamp without time zone not null,
  logged_user character varying(255) not null,
  active boolean not null,
  PRIMARY KEY(sticker_id)
);
CREATE SEQUENCE  ftms.ftms_sticker_seq owned by ftms.ftms_sticker.sticker_id;
ALTER TABLE ftms.ftms_sticker ALTER COLUMN sticker_id SET DEFAULT nextval('ftms.ftms_sticker_seq');

CREATE INDEX ftms_sticker_qr_full_text_active_idx
ON ftms.ftms_sticker (qr_full_text, active);

---Dont run these on a system which already has the merlin db
---FRom public schema. LGAs of all states
CREATE TABLE location_type_upper(  
mr_code character varying(20) PRIMARY KEY,
active boolean NOT NULL,
description character varying(255) NOT NULL,
state character varying(50) not null
);

CREATE TABLE public.setting
(
  mr_code character varying(20) NOT NULL,
  value character varying(200) NOT NULL,
  active boolean NOT NULL,
  description character varying(255),
  CONSTRAINT setting_pkey PRIMARY KEY (mr_code)
);

CREATE TABLE lkp_state(  
	mr_code character varying(50) PRIMARY KEY,
	active boolean NOT NULL,
	description character varying(100) not null
);

--Public Table not under ftms schema
CREATE TABLE public.file_type
(
  file_type_id integer,
  file_type_code character varying(20),
  file_classification_code character varying(20) NOT NULL,
  active boolean NOT NULL,
  description character varying(255),
  min_number integer,
  max_number integer,
  auto_generated boolean NOT NULL,
  current_number integer,
  number_format character varying(100),
  min_size integer NOT NULL,
  certificate_code integer UNIQUE,
  PRIMARY KEY (file_type_id)
);

CREATE SEQUENCE file_type_seq owned by file_type.file_type_id;
ALTER TABLE file_type ALTER COLUMN file_type_id SET DEFAULT nextval('file_type_seq');

CREATE TABLE public.lkp_file_classification
(
  mr_code character varying(20) NOT NULL,
  description character varying(255) NOT NULL,
  active boolean NOT NULL,
  CONSTRAINT lkp_file_classification_pkey PRIMARY KEY (mr_code)
);

CREATE TABLE logger 
(
  ID integer PRIMARY KEY,
  createdate timestamp without time zone,
  machinename character varying(50),
  message character varying(1000),
  category character varying(25)
);
CREATE SEQUENCE  logger_id_seq owned by logger.id;
ALTER TABLE logger ALTER COLUMN id SET DEFAULT nextval('logger_id_seq');

--file table
CREATE TABLE file
(
  file_id integer,
  file_number character varying (90) NOT NULL UNIQUE,
  auto_generated boolean NOT NULL,
  file_alias character varying (90),
  file_type integer NOT NULL,
  owner_id integer,
  file_status_code character varying(20) NOT NULL,
  property_no character varying (20),
  property_size numeric(18,3),
  location_code character varying (20),
  landuse_code character varying (20),
  landpurpose_code character varying(20),
  recordation timestamp without time zone NOT NULL,
  active boolean,
  leaseyear_code integer,
  rofo_exists boolean,
  cofo_exists boolean,
  modified_by integer NOT NULL,
  allocation_status character varying(20),
  transaction_status character varying(20),
  other_claimants_code integer,
  file_number_only integer,
  PRIMARY KEY (file_id)
);

CREATE SEQUENCE file_seq owned by file.file_id;
ALTER TABLE file ALTER COLUMN file_id SET DEFAULT nextval('file_seq');

--file status table
CREATE TABLE file_status
(
  mr_code character varying(20),
  description character varying(100),
  active boolean NOT NULL,
  PRIMARY KEY (mr_code)
);

--party table
CREATE TABLE party(  
party_id integer PRIMARY KEY
);

CREATE SEQUENCE  party_id_seq owned by party.party_id;
ALTER TABLE party ALTER COLUMN party_id SET DEFAULT nextval('party_id_seq');

--appuser table
CREATE TABLE appuser (  
appuser_id integer PRIMARY KEY,  
username character varying (20) NOT NULL,  
first_name character varying(60),
last_name character varying(60),
password character varying (100) NOT NULL,
active boolean NOT NULL,
created_by integer,
created_date timestamp without time zone NOT NULL,
modified_by integer,
modified_date timestamp without time zone NOT NULL
);
CREATE SEQUENCE  appuser_id_seq owned by appuser.appuser_id;
ALTER TABLE appuser ALTER COLUMN appuser_id SET DEFAULT nextval('appuser_id_seq');

----- table 'allocation_status'.
CREATE TABLE allocation_status
(
   mrcode character varying(20) PRIMARY KEY, 
   status character varying(100), 
   active boolean,
   request_category_type character varying(20) NOT NULL
);