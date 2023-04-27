from pydrive.drive import GoogleDrive
from pydrive.auth import GoogleAuth
import sys

gauth = GoogleAuth()           
drive = GoogleDrive(gauth) #Applies google authentication to access a Google drive

upload_file = sys.argv[1] #Accepts an argument for a filename

#Creates an empty file with the location set by naming the parent folder id
#The id of the parent folder is the last part of the url while inside the folder
gfile = drive.CreateFile({'parents': [{'id': '1eMJ8uxc_Aj-EH8wKk-Dgr9TKTY5cCUH4'}]})

gfile.SetContentFile(upload_file) # Read file and set it as the content of this instance.
gfile.Upload() #Upload the file.
