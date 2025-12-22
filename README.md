# Overview
This project is an ASP.NET Core 8 API for managing Suprema biometric devices via gRPC Gateway.
It supports device connection, configuration, user management, authentication, schedules, and real-time operations.
All devices are centrally managed through a unified DeviceManager service.

# V 1.0.0
### Features

- Gateway & Device Management
    - Connect to Suprema Gateway using configurable settings
    - Connect, disconnect, and reconnect multiple devices dynamically
    - Centralized device session management with DeviceManager

- Device Operations
    - Get and set device time
    - Reboot and factory reset devices
    - Manage display settings (language, volume, notice, sounds)

- User & Credential Management
    - User management (add, update, delete, sync)
    - Card enrollment and card scan support
    - Fingerprint and face enrollment

- Authentication & Schedules
    - Configure authentication modes and schedules
    - Create and manage weekly schedules

- Event & Monitoring
    - Real-time monitoring of device events (sign-in logs)
    - Multi-device event subscription support

- API & Architecture
    - ASP.NET Core 8 RESTful API
    - gRPC-based communication with devices
    - Centralized exception handling with unified API response format
    - Configuration-driven setup via appsettings.json