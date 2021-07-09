# Tool for sending UDP packets for port knocking.
You can download compiled version here:</br>
https://sourceforge.net/projects/portknocker-client/files/</br>
</br>
## Simple configuration to secure ftp connection to Mikrotik (192.168.0.1):

### To temporarily open the FTP port (21) to one IP address you currently have:
- knock 4 times on port 5555</br>
- knock 3 times on port 5554</br>
- knock 5 times on port 4553</br>
- knock 4 times on port 1552</br>
After this Mikrotik open connection to knocikng IP address for 5 minutes to new connection</br>
</br>
### PortKnocingClient configuration:
192.168.0.1;5555;4;300</br>
192.168.0.1;5554;3;300</br>
192.168.0.1;4553;5;300</br>
192.168.0.1;1552;4;300</br>
</br>
### Mikrotik firewall configuration:

[admin@MikroTik] > /ip firewall filter print </br>
Flags: X - disabled, I - invalid, D - dynamic </br>
 0    ;;; Drop when on DropList</br>
      chain=input action=drop protocol=tcp src-address-list=DropList dst-port=21 log=yes log-prefix="Drop blocked" </br>
 1    ;;; Adding to droplist when trying connect without Stages      chain=input action=add-src-to-address-list protocol=tcp src-address-list=!Stage-4 address-list=DropList address-list-timeout=10m dst-port=21 log=yes log-prefix="Adding to DropList"</br> 
 2    chain=input action=add-src-to-address-list protocol=udp address-list=Stage-1 address-list-timeout=3s dst-port=5555 nth=4,1 log=yes log-prefix="ToStage1" </br>
 3    chain=input action=add-src-to-address-list protocol=udp src-address-list=Stage-1 address-list=Stage-2 address-list-timeout=3s dst-port=5554 nth=3,1 log=yes log-prefix="ToStage2" </br>
 4    chain=input action=add-src-to-address-list protocol=udp src-address-list=Stage-2 address-list=Stage-3 address-list-timeout=3s dst-port=4553 nth=5,1 log=yes log-prefix="ToStage3" </br>
 5    chain=input action=add-src-to-address-list protocol=udp src-address-list=Stage-3 address-list=Stage-4 address-list-timeout=5m dst-port=1552 nth=4,1 log=yes log-prefix="ToStage4" </br>
 6    chain=input action=accept connection-state=new protocol=tcp src-address-list=Stage-4 dst-port=21 log=yes log-prefix="Allow new connection" </br>
 7    chain=input action=drop connection-state=new protocol=tcp dst-port=21 log=yes log-prefix="Drop new connection" </br>
</br>
</br>
---</br>
Any question/suggestions ?</br>
Let me know ;)</br>
marek@www2.pl</br>
</br>

