# Tool for sending UDP packets for port knocking.

## Simple configuration to secure ftp connection to Mikrotik (192.168.0.1):

To open FTP (21) connection you have to
- knock 4 times on port 5555
- knock 3 times on port 5554
- knock 5 times on port 4553
- knock 4 times on port 1552
After this Mikrotik open connection to knocikng IP address for 5 minutes to new connection

### PortKnocingClient configuration:
192.168.0.1;5555;4;300</br>
192.168.0.1;5554;3;300
192.168.0.1;4553;5;300
192.168.0.1;1552;4;300

### Mikrotik firewall configuration:

[admin@MikroTik] > /ip firewall filter print 
Flags: X - disabled, I - invalid, D - dynamic 
 0    ;;; Drop when on DropList
      chain=input action=drop protocol=tcp src-address-list=DropList dst-port=21 log=yes log-prefix="Drop blocked" 
 1    ;;; Adding to droplist when trying connect without Stages      chain=input action=add-src-to-address-list protocol=tcp src-address-list=!Stage-4 address-list=DropList address-list-timeout=10m dst-port=21 log=yes log-prefix="Adding to DropList" 
 2    chain=input action=add-src-to-address-list protocol=udp address-list=Stage-1 address-list-timeout=3s dst-port=5555 nth=4,1 log=yes log-prefix="ToStage1" 
 3    chain=input action=add-src-to-address-list protocol=udp src-address-list=Stage-1 address-list=Stage-2 address-list-timeout=3s dst-port=5554 nth=3,1 log=yes log-prefix="ToStage2" 
 4    chain=input action=add-src-to-address-list protocol=udp src-address-list=Stage-2 address-list=Stage-3 address-list-timeout=3s dst-port=4553 nth=5,1 log=yes log-prefix="ToStage3" 
 5    chain=input action=add-src-to-address-list protocol=udp src-address-list=Stage-3 address-list=Stage-4 address-list-timeout=5m dst-port=1552 nth=4,1 log=yes log-prefix="ToStage4" 
 6    chain=input action=accept connection-state=new protocol=tcp src-address-list=Stage-4 dst-port=21 log=yes log-prefix="Allow new connection" 
 7    chain=input action=drop connection-state=new protocol=tcp dst-port=21 log=yes log-prefix="Drop new connection" 


---
Any question/suggestions ?
Let me know ;)
marek@www2.pl


