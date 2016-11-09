import os
import time
import traceback
from shutil import copyfile


HOST_PATH = "C:\Windows\System32\drivers\etc\hosts"
BACKUP_PATH = "C:\Windows\System32\drivers\etc\hostsbackup"

try:
	origin_content = ''
	if os.path.isfile(BACKUP_PATH):
		with open(BACKUP_PATH, "r") as _:
			origin_content = _.read()

	with open(HOST_PATH, 'w') as _:
		_.write(origin_content)
	os.remove(BACKUP_PATH)
except Exception, e:
	traceback.print_exc()
