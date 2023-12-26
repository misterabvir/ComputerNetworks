# Урок 7. NAT. GRE.

Задача 0. Настроить сеть согласно схеме в файле, где:
— Office 1 — сеть `10.1.1.0/24`
— Office 2 — сеть `10.0.0.0/16`
— Office 3 — сеть `172.16.0.0/16`
— Office 4 — сеть `192.168.145.0/24`
— Где “Интернет” — там имитация Интернета с помощью OSPF, выберите сами публичные сети между роутерами.

## NETWORK
![alt text](/src/network.png "network")

## office1 & office2 NAT

![alt text](/src/nat.png "NAT")

## office3 & office4 GRE
![alt text](/src/gre.png "GRE")

>ps поздно заметил что настроить NAT необходимо для office 3&2, а GRE для Offices 1&4. Переделывать не стал ввиду идентичности Office1 и Office2.

## openvpn
![alt text](/src/vpn.png "VPN")


