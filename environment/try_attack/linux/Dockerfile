FROM kalilinux/kali-rolling

RUN apt-get -y update && apt-get -y upgrade && \
   DEBIAN_FRONTEND=noninteractive apt-get install -y \
   hping3 \
   slowhttptest \
   siege \
   iputils-ping \
   bash-completion && \
   apt-get autoremove -y && \
   apt-get clean