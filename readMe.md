# ⚔️ Eldoria Chronicles: An Epistolary Health Quest 📜

**Embark on a unique health and wellness journey where your email inbox becomes a portal to the enchanting realm of Eldoria! Receive missives from your fantasy companion, make choices that shape your RPG narrative, and translate real-world healthy habits into epic progress.**

This project is a submission for the **Postmark Challenge: Inbox Innovators**, reimagining email as an interactive gateway to a gamified wellness experience.

## ✨ The World of Eldoria

Eldoria is a realm of breathtaking beauty, ancient magic, and hidden perils, now slowly succumbing to the **Grey Blight** – a mysterious force representing lethargy, self-doubt, and ill-health. As a **Warden of Wellness**, you, the player, are called upon to answer the plea of its denizens. Through your real-world efforts to improve your wellbeing, you'll push back the Blight and restore vitality to yourself and the land, one quest, and one email, at a time.

## 📧 How It Works: Your Email Penpal Awaits!

1.  **The Calling:** Initiate your adventure by sending an email to Eldoria.
2.  **Meet Your Companion:** You'll be partnered with a unique fantasy character (e.g., a steadfast knight, a wise elven scholar, a nimble halfling explorer) powered by AI, who will become your email penpal. This companion, ` {Context.CompanionId} `, will guide you, share their own tales, and react to your progress.
3.  **Embark on Quests:** Your companion will present you with "Restoration Quests" tied to real-world health goals (e.g., "The Sunstone Path" for daily walks, "The Moonpetal Draught" for hydration).
4.  **Epistolary Adventures:**
    * You'll receive emails styled as fantasy scrolls or letters from your companion.
    * Reply to these emails detailing your progress, challenges, or choices.
    * Your real-world actions and replies directly influence your personal RPG narrative, dynamically generated and guided by your AI companion.
5.  **Restore Eldoria:** Achieve milestones, earn (virtual) trophies, unlock lore, and witness the positive impact of your journey on your wellbeing and the fantasy world around you.

## 🚀 Key Features

* **📧 Email-Based Interaction:** Core gameplay loop driven by sending and receiving emails, processed via Postmark webhooks. No separate app needed!
* **🤖 AI-Powered Companion System:** Intelligent and dynamic fantasy companions crafted with Google Gemini, providing unique personalities and narrative generation.
* **🎨 Fantasy Character Image Generation:** Visualize your unique companion with AI-generated avatars, potentially stored via Pinata on IPFS.
* **💾 User Message Tracking & Persistence:** Your journey and interactions are remembered, allowing for a continuous and evolving narrative.
* **🏆 Trophy and Quest System:** Engage in various "Restoration Quests" and earn thematic trophies for achieving your health and wellness goals.
* **🎭 Companion Customization:** Influence the archetype or style of your AI companion to better suit your preferences.

## 🛠️ Technology Stack

* **Backend:** .NET 8.0
* **Data Access:** Entity Framework Core with PostgreSQL
* **AI & LLMs:**
    * Google Gemini
* **Email Processing:** Postmark (Inbound Webhooks are central!)
* **Decentralized File Storage:** Pinata (for potential storage of generated character avatars on IPFS)
* **Containerization:** Docker support

## 🏛️ Architecture

* **Data Access:** Repository Pattern
* **Transaction Management:** Unit of Work Pattern
* **Service Management:** Dependency Injection
* **Deployment:** Containerized with Docker for portability and scalability.

## ⚙️ Getting Started

### Prerequisites

* [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* [Docker Desktop](https://www.docker.com/products/docker-desktop)
* Access to a PostgreSQL instance.
* API Keys / Access for:
    * Postmark (Server Token and configured Inbound Hook)
    * Pinata
* Git

## ✨ Postmark Challenge: Inbox Innovators

This project leverages **Postmark's Inbound Email Processing** as its core interaction mechanism:
* Emails sent to a dedicated Postmark address are parsed.
* This JSON data (containing sender, subject, body, attachments, and `MailboxHash` for routing) is sent via webhook to our .NET application.
* Our application processes this data to identify the user, extract their message/choices, trigger AI companion logic, update the game state, and queue a reply email (sent via Postmark's outbound API).

## 🌟 Future Enhancements

* Deeper branching narratives based on more nuanced player choices.
* Visual map of Eldoria that updates with player progress (perhaps sent as occasional image emails).
* Community features (e.g., anonymized "Warden Reports" sharing collective successes).
* More companion archetypes and quests.
* Integration with other health platforms (with user consent).

## 🧑‍💻 Author(s)

* Sebastian Van Rooyen ([https://dev.to/sebastiandevelops])

## 🙏 Acknowledgements

* Thanks to Postmark for this inspiring hackathon challenge!
* Any other inspirations, libraries, or assets you want to credit.

---
