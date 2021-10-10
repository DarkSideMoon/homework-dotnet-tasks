#!/usr/bin/env /usr/local/opt/python@3.9/bin/python3.9

import sys
import psycopg2
from faker import Faker
from contextlib import closing
from random import randint
import time

if __name__ == "__main__":
    total = 10
    batch_size = 1000
    if len(sys.argv) > 1:
        total = int(sys.argv[1])
        if len(sys.argv) > 2:
           batch_size = int(sys.argv[2])

    fake = Faker(["en_US"], use_weighting=False)

    with closing(psycopg2.connect(dbname='hla', user='postgres',
                                  password='postgres', host='localhost', port = 5432)) as conn:
        with closing(conn.cursor()) as cursor:

            start_time = time.monotonic()

            for i in range(total):
                author = fake.first_name() + " " + fake.last_name()
                title = fake.text(max_nb_chars=20).replace(".", "")
                year = fake.date_between(start_date='-75y', end_date='today').year
                category_id = randint(1,3)

                record = (category_id, author, title, year)

                cursor.execute("INSERT INTO books (id, category_id, author, title, year) VALUES (nextval('books_seq'), %s, %s, %s, %s)", record)

                if i>0 and i%batch_size == 0:
                    print("commit at",i)
                    conn.commit()

            conn.commit()

            elapsed = round(time.monotonic() - start_time, 2)

    print()
    print("Inserted ", total, " records in", elapsed, "sec")