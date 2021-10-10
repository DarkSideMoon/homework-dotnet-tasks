./stop.sh
rm -rf ./data/*
mkdir ./data/master
mkdir ./data/slave
rm -rf ./backups/full/*
rm -rf ./backups/wal/*
./up.sh