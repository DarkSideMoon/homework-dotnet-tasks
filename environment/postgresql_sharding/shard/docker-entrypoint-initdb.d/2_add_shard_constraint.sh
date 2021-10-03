#!/bin/bash
set -e

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-EOSQL
ALTER TABLE books  ADD CONSTRAINT category_id_check CHECK ( category_id % 2 = ${SHARD_INDEX} );
EOSQL