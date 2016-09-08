# Rocket-Mod-ItemRestrictions2


The premise is the same as the original plugin by Lee Murphy (LeeIzaZombie).
Moved here to Github and resurrected... like a zombie mod!

The original project as of this date (9/7/2016) is located here:  https://bitbucket.org/LeeIzaZombie/profile/repositories

This release restores the mod to functional with latest version of Rocket Mod v2.4.8.0
and Unturned v3.16.2.1

Use the sample xml configuration file to fill in the items you want to restrict.  The Item Description tag is just there for user readability/maintenance, it is not used in code.

```xml
  <RestrictedItems>
    <Item>
      <ID>519</ID>
      <Descr>Rocket Launcher</Descr>
    </Item>
    <Item>
      <ID>1100</ID>
      <Descr>Sticky Grenade</Descr>
    </Item>
    <Item>
      <ID>1240</ID>
      <Descr>Detonator</Descr>
    </Item>
  </RestrictedItems>
```

Restricted items are removed from a users inventory whenever they are being added to their inventory. That included equipping an item not already equipped, or even moving it within their inventory.  If they touch it, poof, it's gone.

That does mean that users on an established server will still be walking around with contraband.  Don't worry for long.  Unless it's on their primary or secondary use slot, if they touch it it's gone.

There are two ways to avoid item confiscation by configuration:

If the xml config file setting "IgnoreAdmin" is set to True, then system admins will not be searched.

```xml
<IgnoreAdmin>true</IgnoreAdmin>
```
  
For other users, if  user's group in RocketMod's Permissions.Config.xml has the permission "ItemRestriction.IgnoreRestrictions", they will not be searched.

```xml
    <Group>
      <Id>moderator</Id>
      <DisplayName>Moderator</DisplayName>
      <Color>113CE7</Color>
      <Members>
        <Member>xxxxxxxxxxxxxxxxx</Member>
      </Members>
      <ParentGroup>default</ParentGroup>
      <Permissions>
        <Permission Cooldown="0">heal</Permission>
        <Permission Cooldown="0">tp</Permission>
        <Permission Cooldown="0">tpto</Permission>
        <Permission Cooldown="0">locate</Permission>
        <Permission Cooldown="0">locate.other</Permission>
        <Permission Cooldown="0">investigate</Permission>
        <Permission Cooldown="0">refuel</Permission>
        <Permission Cooldown="0">ItemRestriction.IgnoreRestrictions</Permission>
      </Permissions>
    </Group>
```

