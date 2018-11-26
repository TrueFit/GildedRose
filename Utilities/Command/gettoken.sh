username="$1"
password="$2"

if [ -z "$1" -o -z "$2" ]
then
    echo "Usage:"
    echo "gettoken.sh {username} {password}"
else
	token=$(http POST http://localhost:5000/api/Token/createtoken username='talkersoft' password='password' | jq ".token" | sed 's|\"||g')
	echo "$token"
	echo "Writing to clipboard"
	echo -n "$token" > /dev/clipboard
fi
