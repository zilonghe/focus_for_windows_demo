import os
import time
import traceback
from shutil import copyfile


HOST_PATH = "C:\Windows\System32\drivers\etc\hosts"
BACKUP_PATH = "C:\Windows\System32\drivers\etc\hostsbackup"
PAC_PATH = "C:\\block_list.txt"

try:
	# backup hosts file
	if not os.path.isfile(BACKUP_PATH):
		copyfile(HOST_PATH, BACKUP_PATH)

	# read pac file.
	read_f = open(PAC_PATH, 'r')
	website_list = list()
	for line in read_f.readlines():
		line = line.strip()
		if not len(line) or line.startswith('#'):
			continue
		website_list.append(line)

	# write to hosts file
	with open(HOST_PATH, "a") as _:
		if website_list:
			_.write('\n')
			for each in website_list:
				_.write('127.0.0.1\t' + each + '\n')

except Exception, e:
	traceback.print_exc()
