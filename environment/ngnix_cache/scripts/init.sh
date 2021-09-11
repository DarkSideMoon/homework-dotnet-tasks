for path in ../images/*; do
    file="$(basename -- $path)"
    echo -n "$path -> $file -> "
    echo "curl -s -o /dev/null http://localhost:8081/images/$file"
    curl -s -o /dev/null "http://localhost:8081/images/$file"
done
