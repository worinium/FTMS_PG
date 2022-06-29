
---FTMS_file table constraints
ALTER TABLE ftms.ftms_file
  ADD FOREIGN KEY (lga_code) REFERENCES public.location_type_upper (mr_code) ON UPDATE NO ACTION ON DELETE NO ACTION,
  ADD FOREIGN KEY (current_transaction) REFERENCES ftms.ftms_transaction(transaction_id) ON UPDATE NO ACTION ON DELETE NO ACTION,
  ADD UNIQUE (current_transaction);

---FTMS_Census table constraints
ALTER TABLE ftms.ftms_census
  ADD FOREIGN KEY (location_code) REFERENCES ftms.ftms_locations (mr_code),
  ADD FOREIGN KEY (file_id) REFERENCES ftms.ftms_file (file_id);

---FTMS_request_details table constraints
ALTER TABLE ftms.request_details
  ADD FOREIGN KEY (request_id) REFERENCES ftms.ftms_file_request (request_id),
  ADD FOREIGN KEY (file_id) REFERENCES ftms.ftms_file (file_id);

---FTMS_transaction table constraints
ALTER TABLE ftms.ftms_transaction
  ADD FOREIGN KEY (file_id) REFERENCES ftms.ftms_file (file_id),
  ADD FOREIGN KEY (from_location) REFERENCES ftms.ftms_locations (mr_code),
  ADD FOREIGN KEY (to_location) REFERENCES ftms.ftms_locations (mr_code),
  ADD FOREIGN KEY (file_condition) REFERENCES ftms.lkp_file_condition (mr_code) ON UPDATE NO ACTION ON DELETE NO ACTION;

--ftms_sticker table constraint
ALTER TABLE ftms.ftms_sticker
    ADD FOREIGN KEY (file_id) REFERENCES ftms.ftms_file (file_id) ON UPDATE NO ACTION ON DELETE NO ACTION;

---Location_Type_Upper table constraints
---Dont run these on a system which already has the merlin db
ALTER TABLE public.location_type_upper
  ADD FOREIGN KEY (state) REFERENCES public.lkp_state (mr_code) ON UPDATE NO ACTION ON DELETE NO ACTION;

ALTER TABLE public.file_type
  ADD FOREIGN KEY (file_classification_code) REFERENCES public.lkp_file_classification (mr_code) ON UPDATE NO ACTION ON DELETE NO ACTION;  

--appuser constraints
ALTER TABLE public.appuser
ADD FOREIGN KEY (created_by) 
REFERENCES public.appuser (appuser_id) 
						ON UPDATE NO ACTION 
						ON DELETE NO ACTION,
ADD FOREIGN KEY (modified_by) 
REFERENCES public.appuser (appuser_id) 
						ON UPDATE NO ACTION
						ON DELETE NO ACTION,
ADD UNIQUE (username);

--file constraints
ALTER TABLE file
ADD FOREIGN KEY (file_type) 
REFERENCES file_type (file_type_id) 
                        ON DELETE NO ACTION
                        ON UPDATE NO ACTION,
ADD FOREIGN KEY (owner_id) 
REFERENCES party (party_id) 
                        ON DELETE NO ACTION
                        ON UPDATE NO ACTION,
ADD FOREIGN KEY (file_status_code)
REFERENCES file_status (mr_code)
                        ON DELETE NO ACTION
                        ON UPDATE NO ACTION,
ADD FOREIGN KEY (modified_by)
REFERENCES appuser(appuser_id)
                        ON DELETE NO ACTION
                        ON UPDATE NO ACTION,
ADD FOREIGN KEY (transaction_status)
REFERENCES allocation_status (mrcode)
                        ON DELETE NO ACTION
                        ON UPDATE NO ACTION,
ADD FOREIGN KEY (allocation_status) 
REFERENCES allocation_status(mrcode)
                        ON DELETE NO ACTION
                        ON UPDATE NO ACTION;