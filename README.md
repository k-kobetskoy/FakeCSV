<a href="https://fakecsv20210330141524.azurewebsites.net/">Published on Azure</a>
login: Admin@gmail.com
pass: Admin@123


<h3>THE CHALLENGE</h3>
Build an online service for generating CSV files with fake(dummy) data using Python and Django:
<ul>
<li>
Any user can log in to the system with username and password. You can use generic views provided by Django to implement these features. Registering new users via the admin interface is enough. Note, you do not need to implement a user profile
interface to support password change.</li>
<li>Any logged-in user can create any number of data schemas to create datasets with fake data.</li>
<li>Each such data schema has a name and the list of columns with names and specified data types.</li>
<li>You need to implement different types of data (at least 5 different types):
<ul>
<li>Full name (a combination of first name and last name)</li>
<li>Job</li>
<li>Email</li>
<li>Domain name</li>
<li>Phone number</li>
<li>Company name</li>
<li>Text (with specified range for a number of sentences)</li>
<li>Integer (with specified range)</li>
<li>Address</li>
<li>Date</li>
</ul>
</li>
<li>Users can build the data schema with any number of columns with any type described above. Some types support extra arguments (like a range), others not.</li>
<li>Each column also has its own name (which will be a column header in the CSV file) and order (just a number to manage column order).</li>
<li>After creating the schema, the user should be able to input the number of records he/she needs to generate and press the “Generate data” button.</li>
<li>The interface should show a colored label of generation status for each dataset (processing/ready).</li>
<li>Add a “Download” button for datasets available for download.</li>
</ul>
<h3>MOCKUP</h3>
The page structure can be seen here:<br>
https://www.figma.com/file/GLah5wCMHIyw7hJI4Gekns/CSV-fake-data-generator







