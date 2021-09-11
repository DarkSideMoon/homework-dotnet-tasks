if [ -z "$1" ]; then
  delay=1
  echo "1 - $delay"
else
  delay=$1
  echo "2 - $delay"
fi

for path in ../images/*; do
  file="$(basename -- "$path")"
  echo -n "$path -> $file -> "
  echo "curl -s -o /dev/null http://localhost/images/$file"
  curl -s -o /dev/null "http://localhost/images/$file"
  sleep $delay
done